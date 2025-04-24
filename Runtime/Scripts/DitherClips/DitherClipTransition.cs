using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Presets;
#endif

[CreateAssetMenu(fileName = "DitherClipTransition", menuName = "DitherClips/Transition")]
public class DitherClipTransition : ScriptableObject
{
    private const float DefaultTransitionDuration = 1.2f;
    
    public AnimationClip clip;
    public float duration = DefaultTransitionDuration;
    [Expandable] public DitherClipTransitionConfig config;

    private void OnValidate()
    {
        Debug.LogWarning("Validate on DitherClipTransition");
        #if UNITY_EDITOR
        // DitherClipPicker.RefreshDitherClipMasterlist();
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
                
            if(p.GetTargetTypeName() == typeof(DitherClipTransition).Name)
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
        var foundPreset = AssetDatabase.LoadAssetAtPath<DitherClipTransitionConfig>(path);
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


