using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CustomEditor(typeof(SequencePlayer))]
public class SequencePlayerInspector : Editor
{
    private SequencePlayer player;
    private GUIContent playFromStart;
    private const float buttonWidth = 24f;
    
    private void OnEnable()
    {
        player = (SequencePlayer)target;
        playFromStart = EditorGUIUtility.IconContent("d_PlayButton");
    }
    
    public override void OnInspectorGUI()
    {
        DrawPlayer();
        DrawDefaultInspector();
    }

    private void DrawPlayer()
    {
        if (!Application.isPlaying)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                EditorGUILayout.LabelField("Enter playmode to enable playback controls.");
            EditorGUILayout.Space();
            return;
        }
        
        EditorGUILayout.Space();

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("TIMELINES:", EditorStyles.boldLabel);
            for (int i = 0; i < player.timelineAssets.Count; i++)
            {
                var timeline = player.timelineAssets[i];
                if (timeline == null)
                    continue;
                
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    if (GUILayout.Button(playFromStart, GUILayout.Width(buttonWidth)))
                    {
                        player.Play(timeline);
                    }
                    EditorGUILayout.ObjectField(
                        timeline,
                        typeof(TimelineAsset),
                        false, 
                        null
                        );
                }
            }
        }
    }
}
