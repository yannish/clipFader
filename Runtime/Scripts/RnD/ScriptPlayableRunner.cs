using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class ScriptPlayableRunner : MonoBehaviour
{
    [Range(0f, 1f)]
    public float blendWeight = 0.2f;
    
    public AnimationClip clip;
    private AnimationClipPlayable clipPlayable;
    private AnimationPlayableOutput playableOutput;
    private Animator animator;
    
    public PlayableGraph graph { get; private set; }
    private ScriptPlayable<QuickPlayable> quickPlayableA;
    private ScriptPlayable<QuickPlayable> quickPlayableB;
    private ScriptPlayable<QuickPlayableMixer> quickPlayableMixer;
    
    private ScriptPlayableOutput output;
    
    // public PlayableDirector playableDirector;
    
    void Start()
    {
        if (clip == null)
            return;
        
        // playableDirector.Play();
        
        // if (playableDirector != null)
        //     GraphVisualizerClient.Show(playableDirector.playableGraph);
        
        graph = PlayableGraph.Create($"{gameObject.name} - GRAPH");

        quickPlayableA = ScriptPlayable<QuickPlayable>.Create(graph);
        QuickPlayable quickPlayableBehaviourA = quickPlayableA.GetBehaviour();
        
        quickPlayableB = ScriptPlayable<QuickPlayable>.Create(graph);
        QuickPlayable quickPlayableBehaviourB = quickPlayableB.GetBehaviour();
        
        quickPlayableA.SetDuration(5f);
        quickPlayableB.SetDuration(5f);
        
        quickPlayableMixer = ScriptPlayable<QuickPlayableMixer>.Create(graph);
        quickPlayableMixer.SetInputCount(2);
        
        quickPlayableMixer.ConnectInput(0, quickPlayableA, 0);// ConnectInput(0, quickPlayableBehaviourA, 0);
        quickPlayableMixer.ConnectInput(1, quickPlayableB, 0);

        output = ScriptPlayableOutput.Create(graph, "Script Playable Output");
        output.SetSourcePlayable(quickPlayableMixer);
        
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        
        clipPlayable = AnimationClipPlayable.Create(graph, clip);
        clipPlayable.SetDuration(clip.length);
        
        playableOutput = AnimationPlayableOutput.Create(graph, "Animation Playable Output", animator);
        playableOutput.SetSourcePlayable(clipPlayable);

        
        graph.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!quickPlayableMixer.IsValid())
            return;
        
        quickPlayableMixer.SetInputWeight(0, blendWeight);
        quickPlayableMixer.SetInputWeight(1, 1f - blendWeight);
    }

    private void OnDestroy()
    {
        if(graph.IsValid())
            graph.Destroy();
    }
}
