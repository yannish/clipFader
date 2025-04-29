using UnityEngine;
using UnityEditor;

/*
 * PATHS TO NEEDING UPDATE:
 *
 * 1. Created / deleted a new dither clip.
 * 2. Created / deleted a float variable.
 * 3. Changed the label on a float variable.
 *
 *
 * TODO:
 *
 *  - don't let duplicates be added to these master lists, or check when dictionary is built.
 * 
 */


public class DitherClipAssetModificationProcessor : AssetModificationProcessor
{
    private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
    {
        // Debug.LogWarning($"Deleting asset: {path}");

        bool ditherAssetDeleted = false;
        var clip = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
        if (clip != null)
        {
            Debug.LogWarning($"Dither clip was deleted.");
            ditherAssetDeleted = true;
        }
        
        var transition = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
        if (transition != null)
        {
            Debug.LogWarning($"Dither transition was deleted.");
            ditherAssetDeleted = true;
        }
        
        var duration = AssetDatabase.LoadAssetAtPath<FloatVariable>(path);
        if (duration != null)
        {
            var guid = AssetDatabase.AssetPathToGUID(path);
            var labels = AssetDatabase.GetLabels(new GUID(guid));
            if(labels.Length == 0)
                Debug.LogWarning("... no labels.");
            else
            {
                foreach (var label in labels)
                {
                    // Debug.LogWarning($"label: {label}");
                    if (label == DitherClipPicker.ditherLabelName)
                    {
                        Debug.LogWarning("had the dither label.");
                        ditherAssetDeleted = true;
                    }
                }
            }
            Debug.LogWarning($"Dither duration was deleted.");
            ditherAssetDeleted = true;
        }

        if (ditherAssetDeleted)
        {
            EditorApplication.delayCall += DitherClipPicker.Refresh;
            // EditorApplication.delayCall += () =>
            // {
            //     DitherClipPicker.Refresh();
            // };
        }
        
        // // Delay call so Unity finishes creating the asset before we touch it

        return AssetDeleteResult.DidNotDelete;
    }

    bool CheckLabelsAtPath(string path)
    {
        
        
        return false;
    }
    
    private static string[] OnWillSaveAssets(string[] paths)
    {
        Debug.LogWarning($"Will be saving assets...");

        bool thereWasADitherClipAmongTheModifiedAssets = false;
        Debug.LogWarning("... delayed action");
        foreach(var path in paths)
        {
            var asset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
            if (asset == null)
                continue;
                
            thereWasADitherClipAmongTheModifiedAssets = true;
        }
        
        foreach(var path in paths){}

        if (thereWasADitherClipAmongTheModifiedAssets)
        {
            Debug.LogWarning("... a dither clip was saved!");
            EditorApplication.delayCall += () =>
            {
                DitherClipPicker.Refresh();
            };
        }
        
        return paths;
    }
    
    private static void OnWillCreateAsset(string path)
    {
        // Debug.LogWarning($"Creating asset: {path}");

        if (path.EndsWith(".meta"))
            return;
        
        // if (path.Contains(".meta"))
            // return;
        
        // Only care about .asset files (and remove the .meta from the end)
        // path = path.Replace(".meta", "");

        // Delay call so Unity finishes creating the asset before we touch it
        EditorApplication.delayCall += () =>
        {
            // Debug.LogWarning($"trying to load dither clip at {path}.");
            
            var asset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
            if (asset == null) 
                return;

            Debug.LogWarning($"Dither clip was created.");
            
            // var presetGuids = AssetDatabase.FindAssets("t:Preset");
            // foreach (string guid in presetGuids)
            // {
            //     string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            //     var p = AssetDatabase.LoadAssetAtPath<Preset>(assetPath);
            //     
            //     if(p.GetTargetTypeName() == typeof(DitherClip).Name)
            //         Debug.LogWarning("Found preset for ghostClipTransition");
            //     
            //     if (p != null && p.CanBeAppliedTo(asset))
            //     {
            //         Debug.LogWarning("can apply preset!");
            //         p.ApplyTo(asset);
            //         break;
            //     }
            // }
            //
            // if (asset is DitherClip transition)
            // {
            //     Debug.LogWarning("... was a ghostclipTransition!");
            //     
            //     var preset = AssetDatabase.FindAssets("default t:GhostClipTransitionConfig").FirstOrDefault();
            //     // Step 3: Apply it to your instance
            //     if (preset != null)
            //     {
            //         
            //     }
            //
            //     // Load preset
            //     // var preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/MyThingSettingsPreset.preset");
            //     // if (preset != null && preset.CanBeAppliedTo(asset))
            //     // {
            //     //     preset.ApplyTo(asset);
            //     //     EditorUtility.SetDirty(asset);
            //     //     AssetDatabase.SaveAssets();
            //     //     Debug.Log($"Applied preset to new {nameof(GhostClipTransition)} at {path}");
            //     // }
            // }
            // else
            // {
            //     Debug.LogWarning("... wasn't a ghostclipTransition though.");
            // }
        };
    }
}
