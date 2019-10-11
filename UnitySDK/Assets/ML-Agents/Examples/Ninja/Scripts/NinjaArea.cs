using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;
using UnityEngine.UI;

public class NinjaArea : Area
{
    public NinjaAgent ninjaAgent;
    public Text cumulativeRewardText;

    public override void ResetArea()
    {
        // Place ninja
    }

    void Update()
    {
        cumulativeRewardText.text = ninjaAgent.GetCumulativeReward().ToString("0.00");
    }
}
