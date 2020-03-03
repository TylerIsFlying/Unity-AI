using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveGold : IBehavior
{
    public override bool Execute()
    {
        if (LittleEasyMove.tTargetGold.gold > 0)
        {
            Debug.Log("Found Gold");
            LittleEasyMove.tPlayerGold.gold += LittleEasyMove.tTargetGold.gold;
            LittleEasyMove.tTargetGold.gold = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
