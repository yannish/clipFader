using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class WrappedClipMixer : PlayableBehaviour
{
    public Animator animator;
    
    public PlayableOutput playableOutput;
    
    public AnimationMixerPlayable mixer;
    
    public WrappedAnimator wrappedAnimator;
    
    
    private AnimationClipPlayable[] clipPlayables;    

    public override void OnPlayableCreate(Playable playable)
    {
        var inputCount = playable.GetInputCount();
        var graph = playable.GetGraph();
        Debug.LogWarning($"Created wrappedClipMixer w/ {inputCount} inputs");
        
        mixer = AnimationMixerPlayable.Create(graph, inputCount);
        playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
        clipPlayables = new AnimationClipPlayable[inputCount];
        
        // for (int i = 0; i < inputCount; i++)
        // {
        //     var input = (ScriptPlayable<WrappedClipBehaviour>)playable.GetInput(i);
        //     var behaviour = input.GetBehaviour();
        //     if (behaviour == null)
        //     {
        //         Debug.LogWarning("behaviour's null here though");
        //         continue;
        //     }
        //     // behaviour.clipPlayable = AnimationClipPlayable.Create(graph, behaviour.clip);
        //     mixer.ConnectInput(i, behaviour.clipPlayable, 0);
        // }
        
        playableOutput.SetSourcePlayable(mixer);
    }
    
    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("Wrapped Animator OnGraphStart");
        var inputCount = playable.GetInputCount();
        Debug.LogWarning($"input count in wrapped clip mixer: {inputCount}");
        
        clipPlayables = new AnimationClipPlayable[inputCount];
        
        var graph = playable.GetGraph();
        for (int i = 0; i < inputCount; i++)
        {
            var input = (ScriptPlayable<WrappedClipBehaviour>)playable.GetInput(i);
            var behaviour = input.GetBehaviour();
            if (behaviour == null)
            {
                Debug.LogWarning("behaviour's null here though");
                continue;
            }
            clipPlayables[i] = AnimationClipPlayable.Create(graph, behaviour.clip);
            mixer.ConnectInput(i, clipPlayables[i], 0);
        }
    }
    
    public override void PrepareData(Playable playable, FrameData info)
    {
        Debug.LogWarning("PrepareData");
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var inputCount = playable.GetInputCount();
        // Debug.LogWarning($"input count in wrapped clip mixer: {inputCount}");
        
        for (int i = 0; i < inputCount; i++)
        {
            var wrappedClipPlayable = (ScriptPlayable<WrappedClipBehaviour>)playable.GetInput(i);
            // var wrappedClipBehaviour = inputPlayable.GetBehaviour();
            var clipPlayable = clipPlayables[i];
            var inputWeight = playable.GetInputWeight(i);
            
            clipPlayable.SetTime(wrappedClipPlayable.GetTime());
            mixer.SetInputWeight(i, inputWeight);
            
            // if (inputWeight > 0f)
            // {
            //     Debug.LogWarning(
            //         $"... showing wrapped for clip: {wrappedClipBehaviour.clip.name} " +
            //         $"w/ weight: {inputWeight}"
            //         );
            // }
            
            // if(inputPlayable.GetInputWeight())
            // Debug.LogWarning("ProcessFrame");
        }
    }
}
