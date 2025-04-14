using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class WrappedClipMixer : PlayableBehaviour
{
    public Animator animator;
    
    public PlayableOutput playableOutput;
    
    public AnimationMixerPlayable mixer;

    public override void OnPlayableCreate(Playable playable)
    {
        Debug.LogWarning("Created wrappedClipMixer.");
    }
    
    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("Wrapped Animator OnGraphStart");
    }
    
    public override void PrepareData(Playable playable, FrameData info)
    {
        Debug.LogWarning("PrepareData");
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // Debug.LogWarning("Processing frame");
    }
}
