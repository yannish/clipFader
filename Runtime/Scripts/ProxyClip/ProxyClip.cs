using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class ProxyClip : PlayableAsset
{
    public AnimationClip clip;
    public AnimationClipPlayable clipPlayable;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        clipPlayable = AnimationClipPlayable.Create(graph, clip);
        return clipPlayable;
    }
}
