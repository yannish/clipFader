using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DitherClipCreationUtil
{
    [MenuItem("Assets/AnimationClip => DitherClip", true)]
    static bool ValidateDitherClipMenu()
    {
        bool selectedOnlyAnimationClips = true;
        var selectedAnimationClips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (selectedAnimationClips.Length == 0)
            return false;

        return true;
    }

    [MenuItem("Assets/AnimationClip => DitherClip")]
    static void CreateDitherClips()
    {
        var selectedAnimationClips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        // if (selectedAnimationClips.Length == 0)
        //     return;

        var createdDitherClips = new List<Object>();
        
        foreach (var selectedAnimationClip in selectedAnimationClips)
        {
            if (selectedAnimationClip is AnimationClip animationClip)
            {
                string path = AssetDatabase.GetAssetPath(selectedAnimationClip);
                string directory = Path.GetDirectoryName(path);
                string fileName = selectedAnimationClip.name + "_dc.asset";
                string fullPath = Path.Combine(directory, fileName);

                DitherClipTransition ditherClip = ScriptableObject.CreateInstance<DitherClipTransition>();
                ditherClip.clip = animationClip;
                
                AssetDatabase.CreateAsset(ditherClip, fullPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                createdDitherClips.Add(ditherClip);
            }
            
            EditorUtility.FocusProjectWindow();
            Selection.objects = createdDitherClips.ToArray();
        }
    }
}
