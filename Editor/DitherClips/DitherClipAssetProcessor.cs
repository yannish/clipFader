using System;
using System.Linq;
using System.Text;
using UnityEngine;

using UnityEditor;
using UnityEditor.Presets;

public class DitherClipAssetProcessor// : AssetModificationProcessor
{
    private static void OnWillCreateAsset(string path)
    {
        Debug.LogWarning($"Creating asset: {path}");         
        
        // Only care about .asset files (and remove the .meta from the end)
        path = path.Replace(".meta", "");
        if (!path.EndsWith(".asset"))
            return;
        
        // Delay call so Unity finishes creating the asset before we touch it
        EditorApplication.delayCall += () =>
        {
            var asset = AssetDatabase.LoadAssetAtPath<DitherClip>(path);
            if (asset == null) 
                return;

            var presetGuids = AssetDatabase.FindAssets("t:Preset");
            foreach (string guid in presetGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var p = AssetDatabase.LoadAssetAtPath<Preset>(assetPath);
                
                if(p.GetTargetTypeName() == typeof(DitherClip).Name)
                    Debug.LogWarning("Found preset for ghostClipTransition");
                
                if (p != null && p.CanBeAppliedTo(asset))
                {
                    Debug.LogWarning("can apply preset!");
                    p.ApplyTo(asset);
                    break;
                }
            }
            
            if (asset is DitherClip transition)
            {
                Debug.LogWarning("... was a ghostclipTransition!");
                
                var preset = AssetDatabase.FindAssets("default t:GhostClipTransitionConfig").FirstOrDefault();
                // Step 3: Apply it to your instance
                if (preset != null)
                {
                    
                }

                // Load preset
                // var preset = AssetDatabase.LoadAssetAtPath<Preset>("Assets/Presets/MyThingSettingsPreset.preset");
                // if (preset != null && preset.CanBeAppliedTo(asset))
                // {
                //     preset.ApplyTo(asset);
                //     EditorUtility.SetDirty(asset);
                //     AssetDatabase.SaveAssets();
                //     Debug.Log($"Applied preset to new {nameof(GhostClipTransition)} at {path}");
                // }
            }
            else
            {
                Debug.LogWarning("... wasn't a ghostclipTransition though.");
            }
        };
    }
}
