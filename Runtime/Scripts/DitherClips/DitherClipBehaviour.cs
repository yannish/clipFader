using UnityEngine;
using UnityEngine.Playables;

public class DitherClipBehaviour : PlayableBehaviour
{
    public AnimationClip clip;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (playerData == null)
            return;
        
        DitherClipHandle ditherClipHandle = playerData as DitherClipHandle;
        if (ditherClipHandle == null)
            return;
        
        // ditherClipHandle.timelineFloat = (float) playable.GetTime();
        // ditherClipHandle.frameData = info.weight;
    }
}
