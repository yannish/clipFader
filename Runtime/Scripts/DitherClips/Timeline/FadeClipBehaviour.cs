using UnityEngine;
using UnityEngine.Playables;

public class FadeClipBehaviour : PlayableBehaviour
{
    public float fadeLevel;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        FadeClipHandle fadeClipHandle = playerData as FadeClipHandle;
        if (!fadeClipHandle)
            return;

        fadeClipHandle.Fade(info.effectiveWeight * fadeLevel);
    }
}
