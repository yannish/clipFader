using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DitherClipPicker
{
    public static DitherClipCollection allDitherClips;
    
    public static DitherClipCurveCollection allDitherClipCurves;
    
    public static DitherClipDurationCollection allDitherClipDurations;


    private const string ditherLabelName = "dither";
    
    private const string folderName = "DitherClips";

    private const string path = "Assets/Resources";

    private const string clipsAssetName = "All DitherClips";

    private const string curvesAssetname = "All DitherClipCurves";

    private const string durationsAssetname = "All DitherClipDurations";

    
    public static Dictionary<string, AnimationClip> clipLookup = new Dictionary<string, AnimationClip>();
    
    public static Dictionary<string, DitherClipTransition> curveLookup = new Dictionary<string, DitherClipTransition>();
    
    public static Dictionary<string, FloatVariable> durationLookup = new Dictionary<string, FloatVariable>();

    // #if UNITY_EDITOR
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    // public static void RefreshAllCollections()
    // {
    //     RefreshDitherClipMasterlist();
    //     RefreshDitherClipCurvesMasterList();
    //     RefreshDitherClipDurationMasterList();
    // }
    // #endif
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void LoadMasterLists()
    {
        Debug.LogWarning("Loading dither clip master lists...");
        
        #if UNITY_EDITOR
        RefreshDitherClipMasterlist();
        RefreshDitherClipCurvesMasterList();
        RefreshDitherClipDurationMasterList();
        #endif
        
        allDitherClips = Resources.Load(
            $"{folderName}/{clipsAssetName}",
            typeof(DitherClipCollection)
            ) as DitherClipCollection;
        
        // if(allDitherClips == null)
        //     Debug.LogWarning("... but couldn't find clips master list.");
        // else
        //     Debug.LogWarning("... found clips");

        allDitherClipCurves = Resources.Load(
            $"{folderName}/{curvesAssetname}",
            typeof(DitherClipCurveCollection)
            ) as DitherClipCurveCollection;
        
        // if(allDitherClipCurves == null)
        //     Debug.LogWarning("... but couldn't find curves master list.");
        // else
        //     Debug.LogWarning("... found curves");
        
        allDitherClipDurations = Resources.Load(
            $"{folderName}/{durationsAssetname}", 
            typeof(DitherClipDurationCollection)
            ) as DitherClipDurationCollection;
        
        clipLookup.Clear();
        curveLookup.Clear();
        durationLookup.Clear();

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

        foreach (var duration in allDitherClipDurations.durations)
        {
            durationLookup.Add(duration.name, duration);
        }
    }

    #if UNITY_EDITOR
    [MenuItem("Tools/DitherClips/Generate DitherClips master list")]
    [InitializeOnLoadMethod]
    public static void RefreshDitherClipMasterlist()
    {
        // Debug.LogWarning("Refreshing dither clip masterlist.");
        
        var masterList = ScriptableObject.CreateInstance<DitherClipCollection>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets("t: DitherClip");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
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
        // Debug.LogWarning("Creating dither clip curves masterlist.");
        
        var masterList = ScriptableObject.CreateInstance<DitherClipCurveCollection>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets("t: DitherClipTransition");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
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
    
    [MenuItem("Tools/DitherClips/Generate DitherClipDurations master list")]
    [InitializeOnLoadMethod]
    public static void RefreshDitherClipDurationMasterList()
    {
        // Debug.LogWarning("Creating dither clip durations masterlist.");
        
        var masterList = ScriptableObject.CreateInstance<DitherClipDurationCollection>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets($"t: FloatVariable l: {ditherLabelName}");
        // Debug.LogWarning($"found {allDitherClipsGUIDs.Length} durations.");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<FloatVariable>(path);
            masterList.durations.Add(asset);
        }
        
        if(!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets", "Resources");
        
        if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            AssetDatabase.CreateFolder(path, folderName);
        
        AssetDatabase.DeleteAsset($"{path}/{folderName}/{durationsAssetname}.asset");
        AssetDatabase.CreateAsset(masterList, $"{path}/{folderName}/{durationsAssetname}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    #endif
}
