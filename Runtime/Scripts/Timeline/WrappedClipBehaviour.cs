using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class WrappedClipBehaviour : PlayableBehaviour
{
    public AnimationClip clip;
    public AnimationClipPlayable clipPlayable;

    // public override void OnPlayableDestroy(Playable playable)
    // {
    //     if(clipPlayable.IsValid())
    //         clipPlayable.Destroy();
    // }
}
