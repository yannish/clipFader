using UnityEditor;
using UnityEngine;

public class DitherClipAssetPostProcessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(
        string[] imported, 
        string[] deleted, 
        string[] moved,
        string[] movedFromAssetPaths
        )
    {
        Debug.LogWarning("Post processing assets.");
        DitherClipPicker.RefreshDitherClipMasterlist();
        DitherClipPicker.RefreshDitherClipCurvesMasterList();
        DitherClipPicker.RefreshDitherClipDurationMasterList();
    }
}
