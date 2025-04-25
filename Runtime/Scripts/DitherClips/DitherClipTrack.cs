using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(DitherClipHandle))]
[TrackClipType(typeof(DitherClipPlayableAsset))]
public class DitherClipTrack : TrackAsset //
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DitherClipMixer>.Create(graph, inputCount);
    }
}
