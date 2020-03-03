using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettinGold : IState
{
    public override void Execute()
    {
        if(LittleEasyMove.tTargetGold.gold > 0)
        {
            LittleEasyMove.tPlayerGold.gold += LittleEasyMove.tTargetGold.gold;
            LittleEasyMove.tTargetGold.gold = 0;
        }
    }
    public override bool OnCondition()
    {
        return Vector3.Distance(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position) <= LittleEasyMove.tDistance ? true:false;
    }
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
}
