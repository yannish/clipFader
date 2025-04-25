using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DitherClipDurationCollection", menuName = "ClipFader/DitherClipDurationCollection")]
public class DitherClipDurationCollection : ScriptableObject
{
    public List<FloatVariable> durations = new List<FloatVariable>();
}
