using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Animator))]
public class DitherClipHandle : MonoBehaviour
{
    public bool logDebug;
    public Animator animator;

    private Renderer[] renderers;
    
    public PlayableGraph graph { get; private set; }
    private PlayableOutput playableOutput;
    private AnimationMixerPlayable mixer;

    private AnimationClipPlayable currClipPlayable;
    private AnimationClipPlayable nextClipPlayable;

    private MaterialPropertyBlock _matPropBlock;
    private int shaderPropID;

    private MaterialPropertyBlock MatPropBlock
    {
        get
        {
            if(_matPropBlock == null)
                _matPropBlock = new MaterialPropertyBlock();
            return _matPropBlock;
        }
    }
    
    
    public void Initialize(DitherClipRunner runner)
    {
        shaderPropID = Shader.PropertyToID(runner.shaderPropName);
        
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = null;
        
        graph = PlayableGraph.Create($"{gameObject.name} - Graph");
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
        mixer = AnimationMixerPlayable.Create(graph, 2);
        playableOutput.SetSourcePlayable(mixer);
        mixer.SetInputCount(2);
        graph.Play();

        renderers = GetComponentsInChildren<Renderer>();
        
        GraphVisualizerClient.Show(graph);
    }

    public void SetInitialClip(AnimationClip clip)
    {
        currClipPlayable = AnimationClipPlayable.Create(graph, clip);
        currClipPlayable.SetTime(0f);
        currClipPlayable.Play();
        
        mixer.ConnectInput(0, currClipPlayable, 0);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void SetVisible() => UpdateMaterialFade(1f);

    public void SetHidden() => UpdateMaterialFade(0f);

    
    public void FinishFadeToClipBlending(DitherClipRunner runner)
    {
        Debug.LogWarning("Fade TO clip DONE.");
        
        //... we were fading TO a clip, so we shut down.
        
        mixer.DisconnectInput(0);
        mixer.DisconnectInput(1);
        
        currClipPlayable = AnimationClipPlayable.Create(graph, nextClipPlayable.GetAnimationClip());
        currClipPlayable.SetTime(1f);
        currClipPlayable.Play();

        mixer.ConnectInput(0, currClipPlayable, 0);
        
        graph.DestroyPlayable(nextClipPlayable);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
        
        SetHidden();
    }
    
    public void TickFadeToClipBlending(DitherClipRunner runner)
    {
        var effectiveTime = runner.currBlendDuration - runner.currBlendTimer;
        var normalizedTime = 1f - Mathf.Clamp01(runner.currBlendTimer / runner.currTransition.duration);
        
        var fadeInDither = runner.currTransition.config.fadeInDitherCurve.Evaluate(normalizedTime);
        var fadeInWeight = runner.currTransition.config.fadeInWeightCurve.Evaluate(normalizedTime);

        mixer.SetInputWeight(0, 1f - fadeInWeight);
        mixer.SetInputWeight(1, fadeInWeight);
        
        if(logDebug)
            Debug.LogWarning($"effectiveTime : {effectiveTime}, fadeIn : {fadeInWeight}");
        
        UpdateMaterialFade(fadeInDither);
    }
    
    
    public void TickFadeToClipBlending_NEW(DitherClipRunner runner)
    {
        var effectiveTime = runner.currBlendDuration - runner.currBlendTimer;
        var normalizedTime = 1f - Mathf.Clamp01(runner.currBlendTimer / runner.currBlendDuration);
        
        var fadeInDither = runner.overrideCurves != null ?
            runner.overrideCurves.fadeInDitherCurve.Evaluate(normalizedTime) :
            runner.defaultCurves.fadeInDitherCurve.Evaluate(normalizedTime);
        
        UpdateMaterialFade(fadeInDither);
        
        var fadeInWeight = runner.overrideCurves != null ?
            runner.overrideCurves.fadeInWeightCurve.Evaluate(normalizedTime) :
            runner.defaultCurves.fadeInWeightCurve.Evaluate(normalizedTime);
        
        mixer.SetInputWeight(0, 1f - fadeInWeight);
        mixer.SetInputWeight(1, fadeInWeight);
    }

    public void FinishFadeFromClipBlending(DitherClipRunner runner)
    {
        Debug.LogWarning("Fade FROM clip DONE.");
            
        //... this was fading OUT, so we smash to the last frame of next, 
            
        mixer.DisconnectInput(0);
        mixer.DisconnectInput(1);
            
        currClipPlayable  = AnimationClipPlayable.Create(graph, nextClipPlayable.GetAnimationClip());
        graph.DestroyPlayable(nextClipPlayable);
            
        currClipPlayable.SetTime(1f);
        currClipPlayable.Play();
            
        mixer.ConnectInput(0, currClipPlayable, 0);
            
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
        
        SetVisible();
    }
    
    public void TickFadeFromClipBlending(DitherClipRunner runner)
    {
        var effectiveTime = runner.currBlendDuration - runner.currBlendTimer;
        var normalizedTime = 1f - Mathf.Clamp01(runner.currBlendTimer / runner.currBlendDuration);
        var fadeOutDither = runner.currTransition.config.fadeOutDitherCurve.Evaluate(normalizedTime);
        var fadeOutWeight = runner.currTransition.config.fadeOutWeightCurve.Evaluate(normalizedTime);
        
        UpdateMaterialFade(fadeOutDither);
        
        mixer.SetInputWeight(0, fadeOutWeight);
        mixer.SetInputWeight(1, 1f - fadeOutWeight);
    }

    public void TickFadeFromClipBlending_NEW(DitherClipRunner runner)
    {
        var effectiveTime = runner.currBlendDuration - runner.currBlendTimer;
        var normalizedTime = 1f - Mathf.Clamp01(runner.currBlendTimer / runner.currBlendDuration);
        
        var fadeOutDither = runner.overrideCurves != null ? 
            runner.overrideCurves.fadeOutDitherCurve.Evaluate(normalizedTime) : 
            runner.defaultCurves.fadeOutDitherCurve.Evaluate(normalizedTime);
        
        UpdateMaterialFade(fadeOutDither);
        
        var fadeOutWeight = runner.overrideCurves != null ? 
            runner.overrideCurves.fadeOutWeightCurve.Evaluate(normalizedTime) : 
            runner.defaultCurves.fadeOutWeightCurve.Evaluate(normalizedTime);
        
        mixer.SetInputWeight(0, fadeOutWeight);
        mixer.SetInputWeight(1, 1f - fadeOutWeight);
    }

    
    public void UpdateMaterialFade(float fadeLevel)
    {
        MatPropBlock.SetFloat(shaderPropID, fadeLevel);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(MatPropBlock);
        }
    }

    public void FadeToClip(DitherClipTransition transition)
    {
        nextClipPlayable = AnimationClipPlayable.Create(graph, transition.clip);
        nextClipPlayable.SetDuration(transition.clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Pause();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        //... curved weight:
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void FadeFromClip_NEW(AnimationClip clip)
    {
        // we're fading FROM the current clip.
        //.. next-clip should be run from the start.
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Pause();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        //... curved weight:
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void FadeToClip_NEW(AnimationClip clip)
    {
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Pause();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }
    
    public void FadeFromClip(DitherClipTransition transition)
    {
        // we're fading FROM the current clip.
        //.. next-clip should be run from the start.
        nextClipPlayable = AnimationClipPlayable.Create(graph, transition.clip);
        nextClipPlayable.SetDuration(transition.clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Pause();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        //... curved weight:
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void FadeToClip(AnimationClip clip)
    {
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Pause();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }
    
    public void FadeFromClip(AnimationClip clip)
    {
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Pause();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }
    
    public void FadeToClip(
        AnimationClip clip,
        float blendDuration,
        float fadeInStartTime,
        float fadeInDuration
        )
    {
        // currFadeInStartTime = fadeInStartTime;
        // currFadeInDuration = fadeInDuration;
        
        // we're fading TO next clip.
        //.. next clip should be set part-way complete
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(1f);
        nextClipPlayable.Play();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void FadeFromClip(
        AnimationClip clip,
        float blendDuration,
        float fadeOutStartTime,
        float fadeOutDuration
        )
    {
        // currFadeOutStartTime = fadeOutStartTime;
        // currFadeOutDuration = fadeOutDuration;
        
        // we're fading FROM the current clip.
        //.. next-clip should be run from the start.
        nextClipPlayable = AnimationClipPlayable.Create(graph, clip);
        nextClipPlayable.SetDuration(clip.length);
        nextClipPlayable.SetTime(0f);
        nextClipPlayable.Play();
        
        mixer.ConnectInput(1, nextClipPlayable, 0);
        
        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);
    }

    public void HandleAnimationEvent(AnimationEvent animationEvent)
    {
        
    }

    // public float timelineFloat;
    // public float frameData;
    // public void TimelineTest()
    // {
    //     Debug.LogWarning("timeline test");
    // }
    
    void OnDestroy()
    {
        if(graph.IsValid())
            graph.Destroy();
    }
}
