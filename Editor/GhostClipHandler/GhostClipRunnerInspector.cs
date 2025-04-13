using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DitherClipRunner))]
public class GhostClipRunnerInspector : Editor
{
    private DitherClipRunner runner;
    private GUIContent playFromStart;
    private const float buttonWidth = 24f;

    private void OnEnable()
    {
        runner = (DitherClipRunner)target;
        playFromStart = EditorGUIUtility.IconContent("d_PlayButton");
    }

    public override void OnInspectorGUI()
    {
        DrawClipPlayer();
        DrawDefaultInspector();
    }

    private void DrawClipPlayer()
    {
        if (!Application.isPlaying)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                EditorGUILayout.LabelField("Enter playmode to enable playback controls.");
            EditorGUILayout.Space();
            return;
        }

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("CLIPS:", EditorStyles.boldLabel);
            for (int i = 0; i < runner.clips.Count; i++)
            {
                var clip = runner.clips[i];
                if (clip == null)
                    continue;
                
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button(playFromStart, GUILayout.Width(buttonWidth)))
                    {
                        runner.TransitionToClip(clip);
                        
                        if(runner.logDebug)
                            Debug.LogWarning($"Transitioning to: {clip.name} in {0f}");
                    }
                    EditorGUILayout.LabelField(clip.name);
                }
            }
        }
    }
}
