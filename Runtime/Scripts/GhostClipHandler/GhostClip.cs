using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "GhostClip", menuName = "GhostClip")]
public class GhostClip : PlayableAsset
{
    public AnimationClip clip;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<GhostClipBehaviour>.Create(graph);
        
        GhostClipBehaviour ghostClipBehaviour = playable.GetBehaviour();
        ghostClipBehaviour.clip = clip;

        return playable;
    }
}
