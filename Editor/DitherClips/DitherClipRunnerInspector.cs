using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DitherClipRunner))]
public class DitherClipRunnerInspector : Editor
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
            for (int i = 0; i < runner.DitherClips.Count; i++)
            {
                var ditherClip = runner.DitherClips[i];
                if (ditherClip == null)
                    continue;
                
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button(playFromStart, GUILayout.Width(buttonWidth)))
                    {
                        runner.TransitionToDitherClip(ditherClip);
                        
                        if(runner.logDebug)
                            Debug.LogWarning($"Transitioning to: {ditherClip.name} in {0f}");
                    }
                    EditorGUILayout.LabelField(ditherClip.name);
                }
            }
        }
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("YARN CALLS:", EditorStyles.boldLabel);
            for (int i = 0; i < runner.DitherClipYarnCalls.Count; i++)
            {
                var yarnCall = runner.DitherClipYarnCalls[i];
                if (yarnCall == null)
                    continue;
                //
                // if (yarnCall.clipName == "")
                //     continue;
                //
                // if (!DitherClipPicker.clipLookup.TryGetValue(yarnCall.clipName, out var clip))
                //     continue;
                //
                // DitherClipTransition curves = null;
                // if(DitherClipPicker.curveLookup.TryGetValue(yarnCall.curveName, out var value))
                //     curves = value;
                
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button(playFromStart, GUILayout.Width(buttonWidth)))
                    {
                        runner.CrossFade(yarnCall.clipName, yarnCall.durationName, yarnCall.curveName);
                        
                        // if (yarnCall.transitionDuration > 0f)
                        // {
                        //     runner.CrossFade(clip, yarnCall.transitionDuration, curves);
                        // }
                        // else
                        // {
                        //     runner.CrossFade(clip, curves: curves);
                        // }

                        if(runner.logDebug)
                            Debug.LogWarning($"Transitioning to: {yarnCall.clipName}");
                    }
                    EditorGUILayout.LabelField(yarnCall.clipName);
                }
            }
        }
        
        EditorGUILayout.Space();
    }
}
