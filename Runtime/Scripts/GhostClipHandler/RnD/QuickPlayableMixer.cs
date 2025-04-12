using UnityEngine;
using UnityEngine.Playables;

public class QuickPlayableMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            ScriptPlayable<QuickPlayable> inputPlayable = (ScriptPlayable<QuickPlayable>)playable.GetInput(i);
            QuickPlayable quickPlayable = inputPlayable.GetBehaviour();
            // info.weight 
        }
        
        Playable input = playable.GetInput(0);//.GetB;
        
        if (input is QuickPlayable)
        {
            
        }
    }
}
