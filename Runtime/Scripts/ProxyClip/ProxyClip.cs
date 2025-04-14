using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class ProxyClip : PlayableAsset
{
    public AnimationClip clip;
    public AnimationClipPlayable clipPlayable;
    
    // public Playable proxyPlayable;
    // public ProxyClipBehaviour proxyClipBehaviour;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        clipPlayable = AnimationClipPlayable.Create(graph, clip);
        // proxyPlayable= ScriptPlayable<ProxyClipBehaviour>.Create(graph, 1);
        return clipPlayable;
    }
}
