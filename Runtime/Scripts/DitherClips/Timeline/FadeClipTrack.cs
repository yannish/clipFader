using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(FadeClipHandle))]
[TrackClipType(typeof(FadeClip))]
public class FadeClipTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        // Debug.LogWarning($"Created FadeClipTrack on {go.name}", go);
        
        var fadeClipHandle = go.GetComponentsInChildren<FadeClipHandle>();
        foreach (var fadeClip in fadeClipHandle)
            fadeClip.Initialize();
        return ScriptPlayable<FadeClipMixer>.Create(graph, inputCount);
    }
}
