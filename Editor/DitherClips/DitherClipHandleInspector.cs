using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DitherClipHandle))]
public class DitherClipHandleInspector : Editor
{
    DitherClipHandle handle;
    
    private void OnEnable()
    {
        handle = (DitherClipHandle)target;
        GraphVisualizerClient.ClearGraphs();
        GraphVisualizerClient.Show(handle.graph);
    }

    private void OnDisable()
    {
        GraphVisualizerClient.Hide(handle.graph);
    }
}
