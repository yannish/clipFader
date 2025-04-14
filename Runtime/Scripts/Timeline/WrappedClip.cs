using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class WrappedClip : PlayableAsset
{
    public AnimationClip clip;
    public AnimationClipPlayable clipPlayable;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        Debug.LogWarning("Created wrappedClipPlayable.");
        
        var playable = ScriptPlayable<WrappedClipBehaviour>.Create(graph, 1);
        clipPlayable = AnimationClipPlayable.Create(graph, clip);
        
        WrappedClipBehaviour wrappedClipBehaviour = playable.GetBehaviour();
        wrappedClipBehaviour.clip = clip;
        
        playable.SetInputCount(1);
        clipPlayable.SetInputCount(1);
        
        // graph.Connect(clipPlayable, 0, playable, 0);
        // playable.ConnectInput(0, clipPlayable, 0);
        // clipPlayable.ConnectInput(0, playable, 0);
        
        return clipPlayable;
    }
}
