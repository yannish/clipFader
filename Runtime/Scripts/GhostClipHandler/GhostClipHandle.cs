using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Animator))]
public class GhostClipHandle : MonoBehaviour
{
    public bool logDebug;
    public Animator animator;
    public string shaderPropName = "_Opacity";
    
    private Renderer[] renderers;
    
    private PlayableGraph graph;
    private PlayableOutput playableOutput;
    private AnimationMixerPlayable mixer;

    private AnimationClipPlayable currClipPlayable;
    private AnimationClipPlayable nextClipPlayable;

    private float currFadeOutStartTime;
    private float currFadeOutDuration;
    
    private float currFadeInStartTime;
    private float currFadeInDuration;

    private float currBlendDuration = -1f;
    private float currBlendTimer = -1f;


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
    
    public void Initialize()
    {
        shaderPropID = Shader.PropertyToID(shaderPropName);
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

    public void UpdateMaterialFade(float fadeLevel)
    {
        MatPropBlock.SetFloat(shaderPropID, fadeLevel);
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].SetPropertyBlock(MatPropBlock);
        }
    }
    
    public void TickFadeToClipBlending()
    {
        if (currBlendTimer < 0f)
            return;
        
        currBlendTimer -= Time.deltaTime;
        
        if (currBlendTimer < 0f)
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
            
            // mixer.ConnectInput(0, currClipPlayable, 0);
            
            mixer.SetInputWeight(0, 1f);
            mixer.SetInputWeight(1, 0f);

            currBlendTimer = -1f;
            currBlendDuration = -1f;
            
            SetHidden();

            return;
        }
        
        var effectiveTime = currBlendDuration - currBlendTimer;
        var normalizedTime = 1f - Mathf.Clamp01(currBlendTimer / currBlendDuration);
        var fadeInLevel = GetFadeInLevel(effectiveTime);

        var nextClipWeight = normalizedTime;
        
        mixer.SetInputWeight(0, 1f - nextClipWeight);
        mixer.SetInputWeight(1, nextClipWeight);
        
        if(logDebug)
            Debug.LogWarning($"effectiveTime : {effectiveTime}, fadeIn : {fadeInLevel}");
        
        UpdateMaterialFade(fadeInLevel);

        float GetFadeInLevel(float time)
        {
            float invLerp = Mathf.InverseLerp(
                currFadeInStartTime,
                currFadeInStartTime + currFadeInDuration,
                time
            );
        
            return invLerp;
        }
    }

    public void TickFadeFromClipBlending()
    {
        if (currBlendTimer < 0f)
            return;
        
        currBlendTimer -= Time.deltaTime;
        if (currBlendTimer < 0f)
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
        
            currBlendTimer = -1f;
            currBlendDuration = -1f;
            
            SetVisible();
            
            return;
        }
        
        var effectiveTime = currBlendDuration - currBlendTimer;
        var normalizedTime = 1f - Mathf.Clamp01(currBlendTimer / currBlendDuration);
        var fadeOutLevel = GetFadeOutLevel(effectiveTime);
        
        UpdateMaterialFade(fadeOutLevel);
        
        var nextClipWeight = normalizedTime;
        
        mixer.SetInputWeight(0, 1f - nextClipWeight);
        mixer.SetInputWeight(1, nextClipWeight);
        
        float GetFadeOutLevel(float time)
        {
            float invLerp = Mathf.InverseLerp(
                currFadeOutStartTime,
                currFadeOutStartTime + currFadeOutDuration,
                time
            );
        
            return 1f - invLerp;
        }
    }
    
    public void FadeToClip(
        AnimationClip clip,
        float blendDuration,
        float fadeInStartTime,
        float fadeInDuration
        )
    {
        currBlendTimer = blendDuration;
        currBlendDuration = blendDuration;
        
        currFadeInStartTime = fadeInStartTime;
        currFadeInDuration = fadeInDuration;
        
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
        currBlendTimer = blendDuration;
        currBlendDuration = blendDuration;

        currFadeOutStartTime = fadeOutStartTime;
        currFadeOutDuration = fadeOutDuration;
        
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

    public float timelineFloat;
    public float frameData;
    public void TimelineTest()
    {
        Debug.LogWarning("timeline test");
    }
    
    void OnDestroy()
    {
        if(graph.IsValid())
            graph.Destroy();
    }
}
