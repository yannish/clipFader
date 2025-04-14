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

        // var mixer = AnimationMixerPlayable.Create(graph, inputCount);
        // var animator = go.GetComponent<Animator>();
        // var playableOutput = AnimationPlayableOutput.Create(graph, "Animation", animator);
        // playableOutput.SetSourcePlayable(mixer);
        
        var scriptOutput = ScriptPlayableOutput.Create(graph, "Manual Script Output");
        var scriptMixer = ScriptPlayable<WrappedClipMixer>.Create(graph, inputCount);
        var scriptMixerBehaviouor = scriptMixer.GetBehaviour();
        scriptMixerBehaviouor.animator = go.GetComponent<Animator>();
        // for (int i = 0; i < inputCount; i++)
        // {
        //     Debug.LogWarning($"... connected wrappedClip {i} to its special mixer");
        //     var playable = (ScriptPlayable<WrappedClipBehaviour>)scriptMixer.GetInput(i);
        //     scriptMixer.ConnectInput(i, playable, 0);
        // }
        
        // var mixerBehaviour = scriptMixer.GetBehaviour();
        // var wrappedAnimator = animator.GetComponent<WrappedAnimator>();
        // mixerBehaviour.wrappedAnimator = wrappedAnimator;
        scriptOutput.SetSourcePlayable(scriptMixer);
        
        return scriptMixer;

        
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
