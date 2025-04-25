using UnityEngine;
using UnityEngine.Playables;

public class FadeClip : PlayableAsset
{
    public float fadeLevel = 1f;
    public AnimationCurve fadeCurve = new AnimationCurve
    (
        new Keyframe(0f, 0f),//{ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(1f, 1f)//{ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
    );

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<FadeClipBehaviour>.Create(graph);
        FadeClipBehaviour fadeClipBehaviour = playable.GetBehaviour();
        fadeClipBehaviour.fadeLevel = fadeLevel;
        return playable;
    }
}
