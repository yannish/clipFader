using Codice.CM.Common.Checkin.Partial;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(ProxyAnimator))]
[TrackClipType(typeof(ProxyClip))]
public class ProxyClipTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        Debug.LogWarning("Creating proxyClipTrack");
        
        var mixer = AnimationMixerPlayable.Create(graph, inputCount);
        var proxyAnimator = go.GetComponent<ProxyAnimator>();
        var animator = proxyAnimator.GetComponent<Animator>();
        var playableOutput = AnimationPlayableOutput.Create(graph, "ANIMATION", animator);
        playableOutput.SetSourcePlayable(mixer);
        
        var proxyClipMixer = ScriptPlayable<ProxyClipMixer>.Create(graph);
        // graph.GetRootPlayable().ConnectInput();
        
        return mixer;
    }
}
