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


public class DitherClipAssetModificationProcessor// : AssetModificationProcessor
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

        return AssetDeleteResult.DidNotDelete;
    }

    bool CheckLabelsAtPath(string path)
    {
        
        
        return false;
    }
    
    private static string[] OnWillSaveAssets(string[] paths)
    {
        // Debug.LogWarning($"Will be saving assets...");

        bool thereWasADitherClipAmongTheModifiedAssets = false;
        foreach(var path in paths)
        {
            var asset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
            if (asset == null)
                continue;
                
            thereWasADitherClipAmongTheModifiedAssets = true;
        }

        bool thereWasAFloatVariableAmongThoseSaved = false;
        foreach (var path in paths)
        {
            var floatVariable = AssetDatabase.LoadAssetAtPath<FloatVariable>(path);
            if (floatVariable == null)
                continue;

            // if (!floatVariable.DeveloperDescription.Contains("dither"))
            //     continue;
                
            thereWasAFloatVariableAmongThoseSaved = true;
        }

        bool thereWereDitherCurvesAmongThoseSaved = false;
        foreach (var path in paths)
        {
            var ditherClipTransition = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);
            if (ditherClipTransition == null)
                continue;
            
            thereWereDitherCurvesAmongThoseSaved = true;
        }

        if (
            thereWasADitherClipAmongTheModifiedAssets 
            || thereWasAFloatVariableAmongThoseSaved
            || thereWereDitherCurvesAmongThoseSaved
            )
        {
            Debug.LogWarning("... a dither clip or float variable was saved!");
            EditorApplication.delayCall += () =>
            {
                DitherClipPicker.Refresh();
            };
        }
        
        return paths;
    }
    
    private static void OnWillCreateAsset(string path)
    {
        // Debug.LogWarning($" creating asset : {path}");
        //
        if (path.EndsWith(".meta"))
            return;
        
        // Debug.LogWarning($"... not a meta file.");
        
        // Delay call so Unity finishes creating the asset before we touch it
        EditorApplication.delayCall += () =>
        {
            // Debug.LogWarning($"trying to load dither clip at {path}.");
            
            var ditherClipAsset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
            var ditherClipTransitionAsset = AssetDatabase.LoadAssetAtPath<DitherClipTransition>(path);

            if (ditherClipAsset != null || ditherClipTransitionAsset != null)
            {
                Debug.LogWarning($"Dither asset was created.");
                DitherClipPicker.Refresh();
            }
        };
    }
}
