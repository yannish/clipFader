using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WrappedAnimator))]
public class WrappedAnimatorInspector : Editor
{
    public void OnEnable()
    {
        var wrappedAnimator = (WrappedAnimator)target;
        GraphVisualizerClient.Show(wrappedAnimator.graph);
    }

    public void OnDisable()
    {
        GraphVisualizerClient.ClearGraphs();
    }
}
