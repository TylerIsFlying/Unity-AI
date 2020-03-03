using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : IState
{
    public override void Execute()
    {
        LittleEasyMove.tPlayer.position = Vector3.MoveTowards(LittleEasyMove.tPlayer.position,LittleEasyMove.tTarget.position, 
            LittleEasyMove.tSpeed * Time.deltaTime);
    }
    public override bool OnCondition()
    {
        return Vector3.Distance(LittleEasyMove.tPlayer.position,LittleEasyMove.tTarget.position) > LittleEasyMove.tDistance ? true : false;
    }
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
}
