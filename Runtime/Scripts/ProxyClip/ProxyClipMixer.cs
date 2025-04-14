using UnityEngine;
using UnityEngine.Playables;

public class ProxyClipMixer : PlayableBehaviour
{
    private Playable[] proxyClipPlayables;
    
    public override void OnPlayableCreate(Playable playable)
    {
        var inputCount = playable.GetInputCount();
        proxyClipPlayables = new Playable[inputCount];
        
        Debug.LogWarning($"Created proxy clip mixer w/ {inputCount} inputs");
        
        // for (int i = 0; i < inputCount; i++)
        {
            // var subPlayable = playable.GetInput(i);
            // proxyClipPlayables[i] = playable.GetInput(i);
        // }
    }
    
    public override void OnGraphStart(Playable playable)
    {
        Debug.LogWarning("Graph start for proxy clip mixer");

        var inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            
        }
    }
}
