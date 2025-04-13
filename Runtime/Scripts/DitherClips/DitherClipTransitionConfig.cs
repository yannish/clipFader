using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DitherClipTransitionConfig", menuName = "DitherClips/TransitionConfig")]
public class DitherClipTransitionConfig : ScriptableObject
{
    public const float finalFadeOutWeightDefault = 0.7f;

    public const float fadeOutDitherStartTimeDefault = 0.25f;

    public const float ditherDurationDefault = 0.5f;
    
    public const float fadeOutMinWeightDefault = 0.7f;
    
    public const float fadeInMinWeightDefault = 0.7f;
    
    [Header("FADE OUT:")]
    public AnimationCurve fadeOutWeightCurve = new AnimationCurve
    (
    new Keyframe(0f, 1f),//{ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(1f, finalFadeOutWeightDefault)//{ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
    );
    
    public AnimationCurve fadeOutDitherCurve = new AnimationCurve
    (
        new Keyframe(0f, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(fadeOutDitherStartTimeDefault, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(fadeOutDitherStartTimeDefault + ditherDurationDefault, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(1f, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
    );
    
    
    [Header("FADE IN:")]
    public AnimationCurve fadeInWeightCurve = new AnimationCurve
    (
        new Keyframe(0f, finalFadeOutWeightDefault),
        new Keyframe(1f, 1f)
    );
    
    public AnimationCurve fadeInDitherCurve = new AnimationCurve
    (
        new Keyframe(0f, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(fadeOutDitherStartTimeDefault, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(fadeOutDitherStartTimeDefault + ditherDurationDefault, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
        new Keyframe(1f, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
    );

    private void Reset()
    {
        fadeOutWeightCurve = new AnimationCurve
        (
            new Keyframe(0f, 1f),//{ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(1f, finalFadeOutWeightDefault)//{ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
        );
        
        fadeOutDitherCurve = new AnimationCurve
        (
            new Keyframe(0f, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(fadeOutDitherStartTimeDefault, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(fadeOutDitherStartTimeDefault + ditherDurationDefault, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(1f, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
        );
        
        fadeInWeightCurve = new AnimationCurve
        (
            new Keyframe(0f, finalFadeOutWeightDefault),
            new Keyframe(1f, 1f)
        );
        
        fadeInDitherCurve = new AnimationCurve
        (
            new Keyframe(0f, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(fadeOutDitherStartTimeDefault, 0f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(fadeOutDitherStartTimeDefault + ditherDurationDefault, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f },
            new Keyframe(1f, 1f){ weightedMode = WeightedMode.Both, inWeight = 0f, outWeight = 0f }
        );
    }
}
