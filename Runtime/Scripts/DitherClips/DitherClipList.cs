using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DitherClipList", menuName = "Scriptable Objects/DitherClipList")]
public class DitherClipList : ScriptableObject
{
    public List<DitherClipTransition> clips = new List<DitherClipTransition>(); 
}
