using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(WrappedAnimator))]
[TrackClipType(typeof(WrappedClip))]
public class WrappedClipTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        Debug.LogWarning($"WrappedClip track mixer created on go : {go.name}, inputCount : {inputCount}", go);

        var mixer = AnimationMixerPlayable.Create(graph, inputCount);
        var animator = go.GetComponent<Animator>();
        var playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
        playableOutput.SetSourcePlayable(mixer);
        return mixer;
        
        // var mixer = ScriptPlayable<WrappedClipMixer>.Create(graph, inputCount);
        //
        // var mixerBehaviour = mixer.GetBehaviour();
        // mixerBehaviour.animator = go.GetComponent<Animator>();
        // mixerBehaviour.playableOutput = AnimationPlayableOutput.Create(graph, "Animation", mixerBehaviour.animator);
        // mixerBehaviour.mixer = AnimationMixerPlayable.Create(graph, inputCount);
        //
        // for (int i = 0; i < inputCount; i++)
        // {
        //     var playable = (ScriptPlayable<WrappedClipBehaviour>)mixer.GetInput(i);
        //     if (playable is ScriptPlayable<WrappedClipBehaviour>)
        //     {
        //         Debug.LogWarning("was right!");
        //     }
        //     
        //     var behaviour = playable.GetBehaviour();
        //     if (behaviour == null)
        //     {
        //         Debug.Log("behaviour is null");
        //         continue;
        //     }
        //     
        //     var clipPlayable = AnimationClipPlayable.Create(graph, behaviour.clip);
        //     mixerBehaviour.mixer.ConnectInput(i, clipPlayable, 0);
        // }
        // mixerBehaviour.playableOutput.SetSourcePlayable(mixer);
        //
        // return mixer;
    }
}
