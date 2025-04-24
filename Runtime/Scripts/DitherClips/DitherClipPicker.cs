using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DitherClipPicker
{
    public static DitherClipList allDitherClips;
    
    public static DitherClipCurveList allDitherClipCurves;
    
    private const string folderName = "DitherClips";

    private const string path = "Assets/Resources";

    private const string clipsAssetName = "All DitherClips";

    private const string curvesAssetname = "All DitherClipCurves";
    
    public static Dictionary<string, AnimationClip> clipLookup = new Dictionary<string, AnimationClip>();
    
    public static Dictionary<string, DitherClipTransitionConfig> curveLookup = new Dictionary<string, DitherClipTransitionConfig>();
    
    
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void LoadMasterLists()
    {
        Debug.LogWarning("Loading dither clip master lists...");
        
        allDitherClips = Resources.Load($"{folderName}/{clipsAssetName}", typeof(DitherClipList)) as DitherClipList;
        
        if(allDitherClips == null)
            Debug.LogWarning("... but couldn't find clips master list.");
        else
            Debug.LogWarning("... found clips");

        allDitherClipCurves = Resources.Load($"{folderName}/{curvesAssetname}", typeof(DitherClipCurveList)) as DitherClipCurveList;
        
        if(allDitherClipCurves == null)
            Debug.LogWarning("... but couldn't find curves master list.");
        else
            Debug.LogWarning("... found curves");
        
        clipLookup.Clear();
        curveLookup.Clear();

        foreach (var ditherClip in allDitherClips.clips)
        {
            if (ditherClip.clip == null)
                continue;
            clipLookup.Add(ditherClip.name, ditherClip.clip);
            Debug.LogWarning($"adding clip {ditherClip.name} to lookup.");
        }

        foreach (var ditherCurve in allDitherClipCurves.curves)
        {
            curveLookup.Add(ditherCurve.name, ditherCurve);
        }
    }

    #if UNITY_EDITOR
    [MenuItem("Tools/DitherClips/Generate DitherClips master list")]
    [InitializeOnLoadMethod]
    public static void RefreshDitherClipMasterlist()
    {
        Debug.LogWarning("Refreshing dither clip masterlist.");
        
        var masterList = ScriptableObject.CreateInstance<DitherClipList>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets("t: DitherClipTransition");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
            masterList.clips.Add(asset);
        }
        
        if(!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets", "Resources");
        
        if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            AssetDatabase.CreateFolder(path, folderName);
        
        AssetDatabase.DeleteAsset($"{path}/{folderName}/{clipsAssetName}.asset");
        AssetDatabase.CreateAsset(masterList, $"{path}/{folderName}/{clipsAssetName}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/DitherClips/Generate DitherClipCurves master list")]
    [InitializeOnLoadMethod]
    public static void RefreshDitherClipCurvesMasterList()
    {
        Debug.LogWarning("Creating dither clip curves masterlist.");
        
        var masterList = ScriptableObject.CreateInstance<DitherClipCurveList>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets("t: DitherClipTransitionConfig");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<DitherClipTransitionConfig>(path);
            masterList.curves.Add(asset);
        }
        
        if(!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets", "Resources");
        
        if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            AssetDatabase.CreateFolder(path, folderName);
        
        AssetDatabase.DeleteAsset($"{path}/{folderName}/{curvesAssetname}.asset");
        AssetDatabase.CreateAsset(masterList, $"{path}/{folderName}/{curvesAssetname}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    #endif
}
