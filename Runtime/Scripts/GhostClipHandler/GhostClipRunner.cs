using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Serialization;

// [RequireComponent(typeof(Animator))]
public class GhostClipRunner : MonoBehaviour
{
    [Header("DEBUG:")] public bool logDebug;
    
    [Header("BLENDING:")]
    public float fadeOutStartTime = 1.5f;
    public float fadeOutDuration = 1f;
    public float fadeInStartTime = 0.5f;
    public float fadeInDuration = 1f;
    public float blendDuration = 3f;
    
    [Header("FADING:")]
    [MinMaxRange(0f, 1f)]
    public MinMaxRange fadeOutRange = new MinMaxRange(0.15f, 0.3f);

    [MinMaxRange(0f, 1f)]
    public MinMaxRange fadeInRange = new MinMaxRange(0.7f, 0.85f);

    // [MinMax]
    // public Vector2 blendRange = new Vector2(0.2f, 0.2f);
    
    [Header("CLIPS:")]
    public AnimationClip idleClip;
    public List<AnimationClip> clips;
    
    [Header("CONFIG:")]
    public DirectorUpdateMode updateMode = DirectorUpdateMode.GameTime;
    public float blendTime = 1f;

    private float currBlendTime = -1f; 
    private float currBlendDuration = -1f;    
    //

    [FormerlySerializedAs("fromHandle")] public GhostClipHandle fromClipHandle;
    [FormerlySerializedAs("toHandle")] public GhostClipHandle toClipHandle;
    
    private Animator fromAnimator;
    
    private PlayableGraph graph;
    private PlayableOutput playableOutput;
    private AnimationMixerPlayable mixer;

    private AnimationClipPlayable currClipPlayable;
    private AnimationClipPlayable nextClipPlayable;

    public bool isGhost;
    
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
        // toHandleClone.transform.position += Vector3.right * 2f;

        fromClipHandle = foundAnimator.gameObject.AddComponent<GhostClipHandle>();
        toClipHandle = toHandleClone.gameObject.AddComponent<GhostClipHandle>();

        fromClipHandle.gameObject.name = $"{handleName} - [FROM]";
        toClipHandle.gameObject.name = $"{handleName} - [TO]";
        
        fromClipHandle.Initialize();
        toClipHandle.Initialize();
        
        fromClipHandle.SetInitialClip(idleClip);
        fromClipHandle.SetVisible();
        
        toClipHandle.SetInitialClip(idleClip);
        toClipHandle.SetHidden();
    }

    void InitializeGraphs()
    {
        fromAnimator = GetComponent<Animator>();
        fromAnimator.runtimeAnimatorController = null;
        
        graph = PlayableGraph.Create($"{this.gameObject.name} - Graph");
        graph.SetTimeUpdateMode(updateMode);
        
        playableOutput = AnimationPlayableOutput.Create(graph, "Animation", fromAnimator);
        mixer = AnimationMixerPlayable.Create(graph, 2);
        
        playableOutput.SetSourcePlayable(mixer);
        
        currClipPlayable = AnimationClipPlayable.Create(graph, idleClip);
        currClipPlayable.SetTime(0f);
        currClipPlayable.Play();
        
        mixer.ConnectInput(0, currClipPlayable, 0);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
        
        graph.Play();
        
        GraphVisualizerClient.Show(graph);

        var ghost = Instantiate(gameObject);
        var ghostHandler = ghost.GetComponent<GhostClipRunner>();
        ghostHandler.isGhost = true;
        Destroy(ghostHandler);
    }
    
    void Update()
    {
        fromClipHandle.TickFadeFromClipBlending();
        toClipHandle.TickFadeToClipBlending();
        
        // TickBlending();        
        // if (Input.GetKeyDown(KeyCode.T))
        //     Tick();
    }

    private void TickBlending()
    {
        if (currBlendTime < 0f)
            return;
        
        currBlendTime -= Time.deltaTime;

        if (currBlendTime < 0f)
        {
            /*
             * - disconnect next clip
             */
            Debug.LogWarning("Done with blend.");
            
            mixer.DisconnectInput(0);
            mixer.DisconnectInput(1);
            
            currClipPlayable = AnimationClipPlayable.Create(graph, nextClipPlayable.GetAnimationClip());
            currClipPlayable.SetTime(1f);
            
            mixer.ConnectInput(0, currClipPlayable, 0);
            
            mixer.SetInputWeight(0, 1f);
            mixer.SetInputWeight(1, 0f);
            
            mixer.DisconnectInput(1);
            
            currBlendTime = -1f;
            currBlendDuration = -1f;

            return;
        }
        
        var blendWeight = Mathf.Clamp01(currBlendTime / currBlendDuration);
        
        mixer.SetInputWeight(0, blendWeight);
        mixer.SetInputWeight(1, 1f - blendWeight);
    }
    
    private void Tick()
    {
        Debug.LogWarning("TICK!");
    }

    public void TransitionToClip(AnimationClip clip)
    {
        fromClipHandle.FadeFromClip(
            clip,
            blendDuration,
            fadeOutStartTime,
            fadeOutDuration
        );
        
        toClipHandle.FadeToClip(
            clip,
            blendDuration,
            fadeInStartTime,
            fadeInDuration
            );
    }
    
    public void PlayClip(AnimationClip clip)
    {
        if (clip == null) 
            return;

        currBlendDuration = blendTime;
        currBlendTime = currBlendDuration;
        
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(0f);
        nextClipPlayable.Play();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        // mixer.SetInputWeight(1, 0f);
    }

    public void HandleAnimationEvent(AnimationEvent animationEvent)
    {
        
    }

    private void OnDestroy()
    {
        if(graph.IsValid())
            graph.Destroy();
    }
}
