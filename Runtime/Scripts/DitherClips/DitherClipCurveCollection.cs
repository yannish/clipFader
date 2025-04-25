using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DitherClipCurveCollection", menuName = "ClipFader/DitherClipCurveCollection")]
public class DitherClipCurveCollection : ScriptableObject
{
    public List<DitherClipTransition> curves = new List<DitherClipTransition>();
}
