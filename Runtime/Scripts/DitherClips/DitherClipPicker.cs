using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class DitherClipPicker
{
    public static DitherClipCollection allDitherClips;
    
    public static DitherClipCurveCollection allDitherClipCurves;
    
    public static DitherClipDurationCollection allDitherClipDurations;


    public const string ditherLabelName = "dither";
    
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
    
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void LoadMasterLists()
    {
        Debug.LogWarning("Loading dither clip master lists...");
        
        #if UNITY_EDITOR
        Refresh();
        #endif
        
        allDitherClips = Resources.Load(
            $"{folderName}/{clipsAssetName}",
            typeof(DitherClipCollection)
            ) as DitherClipCollection;

        allDitherClipCurves = Resources.Load(
            $"{folderName}/{curvesAssetname}",
            typeof(DitherClipCurveCollection)
            ) as DitherClipCurveCollection;
        
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

            if (clipLookup.ContainsKey(ditherClip.clip.name))
            {
                Debug.LogWarning($"Already contained a clip by the name: {ditherClip.clip.name}");
                continue;
            }
            
            clipLookup.Add(ditherClip.name, ditherClip.clip);
            Debug.LogWarning($"adding clip {ditherClip.name} to lookup.");
        }

        foreach (var ditherCurve in allDitherClipCurves.curves)
        {
            if (curveLookup.ContainsKey(ditherCurve.name))
            {
                Debug.LogWarning($"Already contained curve by the name: {ditherCurve.name}");
                continue;
            }
            curveLookup.Add(ditherCurve.name, ditherCurve);
        }

        foreach (var duration in allDitherClipDurations.durations)
        {
            if (durationLookup.ContainsKey(duration.name))
            {
                Debug.LogWarning($"Already contained a duration by the name: {duration.name}");
                continue;
            }
            durationLookup.Add(duration.name, duration);
        }
    }

    public static void RefreshAllDitherClipResources()
    {
        #if UNITY_EDITOR
        // RefreshDitherClipMasterlist();
        // RefreshDitherClipCurvesMasterList();
        // RefreshDitherClipDurationMasterList();
        #endif
    }
    
    #if UNITY_EDITOR
    // [MenuItem("Tools/DitherClips/Check Folder Structure")]
    public static void CheckFolderStructure()
    {
        var otherFolders = AssetDatabase.GetSubFolders($"{path}/{folderName}");
        Debug.LogWarning("Sub folders in dither:");
        foreach (var folder in otherFolders)
        {
            Debug.Log(folder);
        }
    }
    #endif
    
    #if UNITY_EDITOR
    [MenuItem("Tools/DitherClips/Refresh DitherClip Caches")]
    public static void Refresh()
    {
        Debug.LogWarning("Refreshing dither clip master lists...");
        
        if(!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets", "Resources");
        
        if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            AssetDatabase.CreateFolder(path, folderName);

        
        //... CLIPS:
        var foundClipCollection = AssetDatabase.LoadAssetAtPath(
            $"{path}/{folderName}/{clipsAssetName}.asset",
            typeof(DitherClipCollection)
        ) as DitherClipCollection;

        if (!foundClipCollection)
        {
            foundClipCollection = ScriptableObject.CreateInstance<DitherClipCollection>();
            AssetDatabase.CreateAsset(foundClipCollection, $"{path}/{folderName}/{clipsAssetName}.asset");
        }
        // else
        //     foundClipCollection.clips.Clear();
        //
        // var allDitherClipsGUIDs = AssetDatabase.FindAssets("t: DitherClip");
        // foreach (var guid in allDitherClipsGUIDs)
        // {
        //     var path = AssetDatabase.GUIDToAssetPath(guid);
        //     var asset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
        //     foundClipCollection.clips.Add(asset);
        // }
        
        // AssetDatabase.DeleteAsset($"{path}/{folderName}/{clipsAssetName}.asset");
        
        
        //... CURVES:
        var foundCurvesCollection = AssetDatabase.LoadAssetAtPath(
            $"{path}/{folderName}/{curvesAssetname}.asset",
            typeof(DitherClipCurveCollection)
        ) as DitherClipCurveCollection;

        if (!foundCurvesCollection)
        {
            foundCurvesCollection = ScriptableObject.CreateInstance<DitherClipCurveCollection>();
            AssetDatabase.CreateAsset(foundCurvesCollection, $"{path}/{folderName}/{curvesAssetname}.asset");
        }
        // else
        //     foundCurvesCollection.curves.Clear();
        
        // var allDitherClipCurveGUIDs = AssetDatabase.FindAssets("t: DitherClipTransition");
        // foreach (var guid in allDitherClipCurveGUIDs)
        // {
        //     var path = AssetDatabase.GUIDToAssetPath(guid);
        //     var asset = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
        //     foundCurvesCollection.curves.Add(asset);
        // }
        
        // if(!AssetDatabase.IsValidFolder(path))
        //     AssetDatabase.CreateFolder("Assets", "Resources");
        //
        // if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        //     AssetDatabase.CreateFolder(path, folderName);

       
        // AssetDatabase.DeleteAsset($"{path}/{folderName}/{curvesAssetname}.asset");
        
        
        //... DURATIONS:
        var foundDurationCollection = AssetDatabase.LoadAssetAtPath(
            $"{path}/{folderName}/{durationsAssetname}.asset",
            typeof(DitherClipDurationCollection)
        ) as DitherClipDurationCollection;

        if (!foundDurationCollection)
        {
            foundDurationCollection = ScriptableObject.CreateInstance<DitherClipDurationCollection>();
            AssetDatabase.CreateAsset(foundDurationCollection, $"{path}/{folderName}/{durationsAssetname}.asset");
        }
        // else
        // {
        //     foundDurationCollection.durations.Clear();
        // }
        //
        // var allDitherDurationsGUIDs = AssetDatabase.FindAssets($"t: FloatVariable");
        // // var allDitherDurationsGUIDs = AssetDatabase.FindAssets($"t: FloatVariable l: {ditherLabelName}");
        // foreach (var guid in allDitherDurationsGUIDs)
        // {
        //     var path = AssetDatabase.GUIDToAssetPath(guid);
        //     var asset = AssetDatabase.LoadAssetAtPath<FloatVariable>(path) as FloatVariable;
        //     if (asset && asset.DeveloperDescription.Contains("dither"))
        //     {
        //         foundDurationCollection.durations.Add(asset);
        //     }
        // }
        
        // if(!AssetDatabase.IsValidFolder(path))
        //     AssetDatabase.CreateFolder("Assets", "Resources");
        //
        // if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
        //     AssetDatabase.CreateFolder(path, folderName);

        // AssetDatabase.DeleteAsset($"{path}/{folderName}/{durationsAssetname}.asset");
        // EditorUtility.SetDirty(foundDurations);
        EditorUtility.SetDirty(foundClipCollection);
        EditorUtility.SetDirty(foundCurvesCollection);
        EditorUtility.SetDirty(foundDurationCollection);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    #endif
    
    
    #if UNITY_EDITOR
    // [MenuItem("Tools/DitherClips/Generate DitherClips master list")]
    // [InitializeOnLoadMethod]
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

    // [MenuItem("Tools/DitherClips/Generate DitherClipCurves master list")]
    // [InitializeOnLoadMethod]
    public static void RefreshDitherClipCurvesMasterList()
    {
        // Debug.LogWarning("Creating dither clip curves masterlist.");
        
        var masterCurvesList = ScriptableObject.CreateInstance<DitherClipCurveCollection>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets("t: DitherClipTransition");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
            masterCurvesList.curves.Add(asset);
        }
        
        if(!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets", "Resources");
        
        if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            AssetDatabase.CreateFolder(path, folderName);
        
        AssetDatabase.DeleteAsset($"{path}/{folderName}/{curvesAssetname}.asset");
        AssetDatabase.CreateAsset(masterCurvesList, $"{path}/{folderName}/{curvesAssetname}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    // [MenuItem("Tools/DitherClips/Generate DitherClipDurations master list")]
    // [InitializeOnLoadMethod]
    public static void RefreshDitherClipDurationMasterList()
    {
        // Debug.LogWarning("Creating dither clip durations masterlist.");
        
        var masterDurationsList = ScriptableObject.CreateInstance<DitherClipDurationCollection>();

        var allDitherClipsGUIDs = AssetDatabase.FindAssets($"t: FloatVariable l: {ditherLabelName}");
        // Debug.LogWarning($"found {allDitherClipsGUIDs.Length} durations.");
        foreach (var guid in allDitherClipsGUIDs)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<FloatVariable>(path);
            masterDurationsList.durations.Add(asset);
        }
        
        if(!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder("Assets", "Resources");
        
        if(!AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            AssetDatabase.CreateFolder(path, folderName);
        
        AssetDatabase.DeleteAsset($"{path}/{folderName}/{durationsAssetname}.asset");
        AssetDatabase.CreateAsset(masterDurationsList, $"{path}/{folderName}/{durationsAssetname}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    #endif
}
