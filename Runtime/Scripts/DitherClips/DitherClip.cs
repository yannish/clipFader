using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Presets;
#endif

[CreateAssetMenu(fileName = "DitherClip", menuName = "ClipFader/DitherClip")]
public class DitherClip : ScriptableObject
{
    private const float DefaultTransitionDuration = 1.2f;
    
    public AnimationClip clip;
    public float duration = DefaultTransitionDuration;
    public float startTime = 0f;
    public bool isAdditive;
    [ShowIf("isAdditive")] public AnimationClip referencePoseClip;
    [ShowIf("isAdditive")] public float referencePoseTime;
    [Expandable] public DitherClipTransition config;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        if (isAdditive && referencePoseClip != null)
        {
            // Debug.LogWarning(".. set ref pose clip.");
            AnimationUtility.SetAdditiveReferencePose(clip, referencePoseClip, referencePoseTime);
        }
        else
        {
            // Debug.LogWarning(".. unset ref pose clip.");
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.hasAdditiveReferencePose = false;
            settings.additiveReferencePoseClip = null;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
        }
        #endif
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Check for preset")]
    public static void CheckForPreset()
    {
        var presetGuids = AssetDatabase.FindAssets("t:Preset");
        foreach (string guid in presetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var p = AssetDatabase.LoadAssetAtPath<Preset>(assetPath);
                
            if(p.GetTargetTypeName() == typeof(DitherClip).Name)
                Debug.LogWarning("Found preset for ghostClipTransition", p);
                
            // if (p != null && p.CanBeAppliedTo(asset))
            // {
            //     Debug.LogWarning("can apply preset!");
            //     p.ApplyTo(asset);
            //     break;
            // }
        }
    }
    #endif
    
    #if UNITY_EDITOR
    [MenuItem("Tools/Create MyThingSettings With Preset")]
    public static void CreateWithPreset()
    {
        // Step 1: Create a new instance of your ScriptableObject
        // var instance = ScriptableObject.CreateInstance<MyThingSettings>();

        // Step 2: Load the preset (adjust path as needed)
        // var preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/MyThingSettingsPreset.preset");
        var allConfigs = AssetDatabase.FindAssets("t:GhostClipTransitionConfig");
        foreach (var guid in allConfigs)
        {
            
        }
        var preset = AssetDatabase.FindAssets("default t:GhostClipTransitionConfig").FirstOrDefault();
        var path = AssetDatabase.GUIDToAssetPath(preset);
        var foundPreset = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
        // Step 3: Apply it to your instance
        if (foundPreset != null)// && preset.CanBeAppliedTo(instance))
        {
            Debug.Log("Found preset.", foundPreset);
            // preset.ApplyTo(instance);
        }
        else
        {
            Debug.LogWarning("Preset not found or can't be applied.");
        }
    }
    #endif
}


