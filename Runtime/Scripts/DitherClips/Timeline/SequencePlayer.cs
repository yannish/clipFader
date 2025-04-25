using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SequencePlayer : MonoBehaviour
{
    public PlayableDirector director;
    public List<TimelineAsset> timelineAssets;

    public void Play(TimelineAsset timelineAsset)
    {
        director.Play(timelineAsset);
    }
    
    void Start()
    {
        // Play(timelineAssets[0]);
    }

    void Update()
    {
        
    }
}
