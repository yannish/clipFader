using UnityEngine;
using UnityEngine.Playables;

public class FadeClipMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        FadeClipHandle fadeClipHandle = playerData as FadeClipHandle;
        if (!fadeClipHandle)
            return;
        
        
    }
}
