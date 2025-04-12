using UnityEngine;
using UnityEngine.Playables;

public class GhostClipBehaviour : PlayableBehaviour
{
    public AnimationClip clip;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        GhostClipHandle ghostClipHandle = playerData as GhostClipHandle;
        ghostClipHandle.timelineFloat = (float) playable.GetTime();
        ghostClipHandle.frameData = info.weight;
    }
}
