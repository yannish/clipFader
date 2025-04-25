using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

// [CreateAssetMenu(fileName = "DitherClip", menuName = "Dither Clip")]
public class DitherClipPlayableAsset : PlayableAsset
{
    public static string toolName = "DitherClip";
    
    public AnimationClip clip;
    
    [FormerlySerializedAs("transitionConfig")]
    [Expandable]
    public DitherClipTransition transition;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DitherClipBehaviour>.Create(graph);
        
        DitherClipBehaviour ditherClipBehaviour = playable.GetBehaviour();
        ditherClipBehaviour.clip = clip;

        return playable;
    }
}
