using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "DitherClip", menuName = "Dither Clip")]
public class DitherClip : PlayableAsset
{
    public static string toolName = "DitherClip";
    
    public AnimationClip clip;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DitherClipBehaviour>.Create(graph);
        
        DitherClipBehaviour ditherClipBehaviour = playable.GetBehaviour();
        ditherClipBehaviour.clip = clip;

        return playable;
    }
}
