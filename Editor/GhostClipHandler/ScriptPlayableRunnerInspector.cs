using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptPlayableRunner))]
public class ScriptPlayableRunnerInspector : Editor
{
    ScriptPlayableRunner scriptPlayableRunner;
    
    private void OnEnable()
    {
        scriptPlayableRunner = (ScriptPlayableRunner)target;
        GraphVisualizerClient.ClearGraphs();
        GraphVisualizerClient.Show(scriptPlayableRunner.graph);
    }

    private void OnDisable()
    {
        GraphVisualizerClient.Hide(scriptPlayableRunner.graph);
    }
}
