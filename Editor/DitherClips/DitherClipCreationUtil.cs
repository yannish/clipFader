using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DitherClipCreationUtil
{
    private const string ToolMenuSlot = "Assets/ClipFader";
    
    private const string CreateDitherClipsDisplayName = "AnimationClip(s) >>> DitherClip(s)";
    
    [MenuItem(ToolMenuSlot + "/" + CreateDitherClipsDisplayName, true)]
    static bool ValidateCreateDitherClips()
    {
        var selectedAnimationClips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (selectedAnimationClips.Length == 0)
            return false;

        return true;
    }

    [MenuItem(ToolMenuSlot + "/" + CreateDitherClipsDisplayName)]
    static void CreateDitherClips()
    {
        var selectedAnimationClips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (selectedAnimationClips.Length == 0)
            return;

        var createdDitherClips = new List<Object>();
        
        foreach (var selectedAnimationClip in selectedAnimationClips)
        {
            if (selectedAnimationClip is AnimationClip animationClip)
            {
                string path = AssetDatabase.GetAssetPath(selectedAnimationClip);
                string directory = Path.GetDirectoryName(path);
                string fileName = selectedAnimationClip.name + "_dc.asset";
                string fullPath = Path.Combine(directory, fileName);

                DitherClip ditherClip = ScriptableObject.CreateInstance<DitherClip>();
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


    private const string CreateDitherClipCollectionDisplayName = "DitherClip(s) >>> DitherClipCollection";
    
    [MenuItem(ToolMenuSlot + "/" + CreateDitherClipCollectionDisplayName, true)]
    static bool ValidateCreateDitherClipCollection()
    {
        var selectedDitherClips = Selection.GetFiltered(typeof(DitherClip), SelectionMode.Assets);
        if (selectedDitherClips.Length == 0)
            return false;

        return true;
    }
    
    [MenuItem(ToolMenuSlot + "/" + CreateDitherClipCollectionDisplayName)]
    static void CreateDitherClipCollection()
    {
        Debug.LogWarning("Creating ditherclip collection from other ditherclips.");
    }
    
    
    
    private const string CreateDitherClipCollectionFromAnimationClipsDisplayName = "AnimationClip(s) >>> DitherClipCollection";
    
    [MenuItem(ToolMenuSlot + "/" + CreateDitherClipCollectionFromAnimationClipsDisplayName, true)]
    static bool ValidateCreateDitherClipCollectionFromAnimationClips()
    {
        var selectedAnimationClips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Assets);
        if (selectedAnimationClips.Length == 0)
            return false;

        return true;
    }

    [MenuItem(ToolMenuSlot + "/" + CreateDitherClipCollectionFromAnimationClipsDisplayName)]
    static void CreateDitherClipCollectionFromAnimationClips()
    {
        Debug.LogWarning("Creating ditherclip collection from animation clips.");
    }
}
