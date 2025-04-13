using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class DitherClipRunner : MonoBehaviour
{
    [Header("DEBUG:")] public bool logDebug;

    [Header("STATE:")] public DitherClipTransition currTransition;
    
    [Header("CLIPS:")] public List<DitherClipTransition> DitherClipTransitions = new List<DitherClipTransition>();
    
    // [Header("FADING:")]
    // [MinMaxRange(0f, 1f)]
    // public MinMaxRange fadeOutRange = new MinMaxRange(0.15f, 0.3f);
    // [MinMaxRange(0f, 1f)]
    // public MinMaxRange fadeInRange = new MinMaxRange(0.7f, 0.85f);
    
    [Header("CLIPS:")]
    public AnimationClip idleClip;
    // public List<AnimationClip> clips;
    
    [Header("CONFIG:")]
    public DirectorUpdateMode updateMode = DirectorUpdateMode.GameTime;

    private DitherClipHandle fromClipHandle;
    private DitherClipHandle toClipHandle;
    
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

        fromClipHandle = foundAnimator.gameObject.AddComponent<DitherClipHandle>();
        toClipHandle = toHandleClone.gameObject.AddComponent<DitherClipHandle>();

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
        var ghostHandler = ghost.GetComponent<DitherClipRunner>();
        ghostHandler.isGhost = true;
        Destroy(ghostHandler);
    }
    
    void Update()
    {
        fromClipHandle.TickFadeFromClipBlending(this);
        toClipHandle.TickFadeToClipBlending(this);
        
        // TickBlending();       
        
        // if (Input.GetKeyDown(KeyCode.T))
        //     Tick();
    }

    private void Tick()
    {
        Debug.LogWarning("TICK!");
    }

    public void TransitionToDitherClip(DitherClipTransition transition)
    {
        currTransition = transition;
        
        fromClipHandle.FadeFromClip(transition);
        toClipHandle.FadeToClip(transition);
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
