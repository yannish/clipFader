using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DitherClipCollection", menuName = "ClipFader/DitherClipCollection")]
public class DitherClipCollection : ScriptableObject
{
    public List<DitherClip> clips = new List<DitherClip>(); 
}
