using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DitherClipCurveList", menuName = "Scriptable Objects/DitherClipCurveList")]
public class DitherClipCurveList : ScriptableObject
{
    public List<DitherClipTransitionConfig> curves = new List<DitherClipTransitionConfig>();
}
