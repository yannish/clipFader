using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class DitherClipRunner : MonoBehaviour
{
    private const float defaultDuration = 1.2f;
    
    [Header("DEBUG:")] 
    public bool logDebug;

    
    [Header("STATE:")] 
    public float currBlendTimer;
    
    public float currBlendDuration;

    
    [Header("CONFIG:")] 
    public AnimationClip idleClip;

    public DitherClip idleAdditiveClip;
    
    public DitherClipTransition defaultCurves;
    
    [Expandable] public DitherClipCollection ditherClipCollection;

    [Expandable] public DitherClipCurveCollection ditherCurvesCollection;
    
    [FormerlySerializedAs("DitherClipTransitions")]
    public List<DitherClip> DitherClips = new List<DitherClip>();
    
    public List<DitherClipYarnCall> DitherClipYarnCalls = new List<DitherClipYarnCall>();

    
    [Header("MATERIALS:")]
    public string shaderPropName = "_Opacity";

    
    private Dictionary<string, AnimationClip> localClipLookup = new Dictionary<string, AnimationClip>();
    private Dictionary<string, DitherClipTransition> curvesLookup = new Dictionary<string, DitherClipTransition>();
    
    private AnimationClip queuedClip;
    private float queuedTransitionDuration;
    private DitherClipTransition queuedTransition;
    public DitherClipTransition overrideCurves { get; private set; }
    
    private DitherClipHandle fromClipHandle;
    private DitherClipHandle toClipHandle;
    
    private Animator fromAnimator;
    
    private PlayableGraph graph;
    private PlayableOutput playableOutput;
    private AnimationMixerPlayable mixer;

    private AnimationClipPlayable currClipPlayable;
    private AnimationClipPlayable nextClipPlayable;

    public DitherClip currDitherClip { get; private set; }
    private AnimationClip currClip;
    private DitherClip queuedDitherClip;

    [HideInInspector] public bool isGhost { get; set; }
    
    
    void Start()
    {
        if (isGhost)
        {
            Debug.Log("this is the ghost, don't set it up");
            return;
        }

        var foundAnimator = GetComponentInChildren<Animator>();
        if (foundAnimator == null)
        {
            Debug.LogWarning("GhostClipRunner couldn't find a child animator component.", this.gameObject);
            return;
        }

        var handleName = foundAnimator.gameObject.name;
        
        var toHandleClone = Instantiate(foundAnimator.gameObject, this.transform, true);

        fromClipHandle = foundAnimator.gameObject.AddComponent<DitherClipHandle>();
        toClipHandle = toHandleClone.gameObject.AddComponent<DitherClipHandle>();

        fromClipHandle.gameObject.name = $"{handleName} - [FROM]";
        toClipHandle.gameObject.name = $"{handleName} - [TO]";
        
        fromClipHandle.Initialize(this);
        toClipHandle.Initialize(this);
        
        fromClipHandle.SetInitialClip(idleClip);
        fromClipHandle.SetVisible();
        
        toClipHandle.SetInitialClip(idleClip);
        toClipHandle.SetHidden();

        currBlendTimer = -1f;
        currBlendDuration = -1f;

        if (ditherClipCollection != null)
        {
            localClipLookup.Clear();
            foreach(var ditherClip in ditherClipCollection.clips)
                localClipLookup.Add(ditherClip.name, ditherClip.clip);
        }
        
        if (ditherCurvesCollection != null)
        {
            curvesLookup.Clear();
            foreach(var ditherCurves in ditherCurvesCollection.curves)
                curvesLookup.Add(ditherCurves.name, ditherCurves);
        }
    }
    
    void Update()
    {
        if (currBlendTimer < 0f)
            return;
        
        currBlendTimer -= Time.deltaTime;

        if (
            (currClip != null || currDitherClip != null )
            && currBlendTimer <= 0f
            )
        {
            //... wrap up...
            fromClipHandle.FinishFadeFromClipBlending(this);
            toClipHandle.FinishFadeToClipBlending(this);
            
            currBlendTimer = -1f;
            currBlendDuration = -1f;

            currClip = null;
            currDitherClip = null;
            overrideCurves = null;
            
            if (queuedClip != null)
            {
                float transitionTime = defaultDuration;
                if(queuedTransitionDuration > 0f)
                    transitionTime = queuedTransitionDuration;

                DitherClipTransition transition = defaultCurves;
                if(queuedTransition != null)
                    transition = queuedTransition;
                
                CrossFade(queuedClip, transitionTime, transition);

                queuedTransitionDuration = -1f;
                queuedClip = null;
                queuedTransition = null;
            }
            
            // if (queuedTransition != null)
            // {
            //     TransitionToDitherClip(queuedTransition);
            //     queuedTransition = null;
            // }

            return;
        }
        
        // fromClipHandle.TickFadeFromClipBlending(this);
        // toClipHandle.TickFadeToClipBlending(this);
        
        fromClipHandle.TickFadeFromClipBlending_NEW(this);
        toClipHandle.TickFadeToClipBlending_NEW(this);
    }


    public void CrossFade(string clipName, string durationName = "", string curvesName = "")
    {
        if (!localClipLookup.TryGetValue(clipName, out var clip))
        {
            Debug.LogWarning($"... couldn't find clip '{clipName}' in runner '{gameObject.name}'s collection.", this.gameObject);
            // if(DitherClipPicker.clipLookup.TryGetValue(clipName, out clip))
                // Debug.LogWarning("... but found it in the master clip list.");
            // else
                return;
        }

        float duration = defaultDuration;
        if (
            durationName != ""
            && DitherClipPicker.durationLookup.TryGetValue(durationName, out var foundDuration)
        )
        {
            duration = foundDuration.Value;
            Debug.LogWarning("... found duration in the master durations list.");
        }
        
        if (
            curvesName != ""
            && !curvesLookup.TryGetValue(curvesName, out var curves)
        )
        {
            Debug.LogWarning($"... couldn't find curves '{curvesName}' in runner '{gameObject.name}'s collection.", this.gameObject);
            if(DitherClipPicker.curveLookup.TryGetValue(curvesName, out var curve))
                Debug.LogWarning("... but found it in the master curves list.");
        }
        else
        {
            curves = defaultCurves;
        }
        
        CrossFade(clip, duration, curves);
    }
    
    public void CrossFade(string clipName, float duration = defaultDuration, string curvesName = "")
    {
        if (!localClipLookup.TryGetValue(clipName, out var clip))
        {
            Debug.LogWarning($"... couldn't find clip '{clipName}' in runner '{gameObject.name}'s collection.", this.gameObject);
            // if(DitherClipPicker.clipLookup.TryGetValue(clipName, out clip))
            //     Debug.LogWarning("... but found it in the master clip list.");
            // else
                return;
        }

        if (
            curvesName != ""
            && !curvesLookup.TryGetValue(curvesName, out var curves)
            )
        {
            Debug.LogWarning($"... couldn't find curves '{curvesName}' in runner '{gameObject.name}'s collection.", this.gameObject);
            if(DitherClipPicker.curveLookup.TryGetValue(curvesName, out var curve))
                Debug.LogWarning("... but found it in the master curves list.");
        }
        else
        {
            curves = defaultCurves;
        }
        
        if(duration <= 0)
            duration = defaultDuration;
        
        CrossFade(clip, duration, curves);
    }
    
    public void CrossFade(AnimationClip clip, float duration = defaultDuration, DitherClipTransition curves = null)
    {
        if (clip == currClip)
            return;

        if(curves != null)
            overrideCurves = curves;
        
        if (currClip != null)
        {
            Debug.LogWarning("already transitioning, queuing next instead.");
            queuedClip = clip;
            queuedTransitionDuration = duration;
            queuedTransition = curves;
            return;
        }

        currClip = clip;
        currBlendTimer = duration;
        currBlendDuration = duration;
        
        fromClipHandle.FadeFromClip_NEW(clip);
        toClipHandle.FadeToClip_NEW(clip);
    }
    
    public void TransitionToDitherClip(DitherClip transition)
    {
        if (transition == currDitherClip)
            return;
        
        if (currDitherClip != null)
        {
            Debug.LogWarning("already transitioning, queueing next instead.");
            queuedDitherClip = transition;
            return;
        }
        
        currDitherClip = transition;
        currBlendTimer = transition.duration;
        currBlendDuration = transition.duration;
        
        fromClipHandle.FadeFromClip(transition);
        toClipHandle.FadeToClip(transition);
    }
    
    public void HandleAnimationEvent(AnimationEvent animationEvent){}

    private void OnDestroy()
    {
        if(graph.IsValid())
            graph.Destroy();
    }
    
    
    // void InitializeGraphs()
    // {
    //     fromAnimator = GetComponent<Animator>();
    //     fromAnimator.runtimeAnimatorController = null;
    //     
    //     graph = PlayableGraph.Create($"{this.gameObject.name} - Graph");
    //     graph.SetTimeUpdateMode(updateMode);
    //     
    //     playableOutput = AnimationPlayableOutput.Create(graph, "Animation", fromAnimator);
    //     mixer = AnimationMixerPlayable.Create(graph, 2);
    //     
    //     playableOutput.SetSourcePlayable(mixer);
    //     
    //     currClipPlayable = AnimationClipPlayable.Create(graph, idleClip);
    //     currClipPlayable.SetTime(0f);
    //     currClipPlayable.Play();
    //     
    //     mixer.ConnectInput(0, currClipPlayable, 0);
    //     
    //     mixer.SetInputWeight(0, 1f);
    //     mixer.SetInputWeight(1, 0f);
    //     
    //     graph.Play();
    //     
    //     GraphVisualizerClient.Show(graph);
    //
    //     var ghost = Instantiate(gameObject);
    //     var ghostHandler = ghost.GetComponent<DitherClipRunner>();
    //     ghostHandler.isGhost = true;
    //     
    //     Destroy(ghostHandler);
    // }
}
