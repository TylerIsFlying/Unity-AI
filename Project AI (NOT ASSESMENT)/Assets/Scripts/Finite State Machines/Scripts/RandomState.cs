using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomState : IState
{
    public override void Execute()
    {
        LittleEasyMove.tTarget.position = new Vector3(
            Random.Range(LittleEasyMove.tMin.x, LittleEasyMove.tMax.x),
            LittleEasyMove.tTarget.position.y,
            Random.Range(LittleEasyMove.tMin.z, LittleEasyMove.tMax.z)
            );
    }
    public override bool OnCondition()
    {
        return Vector3.Distance(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position) <= LittleEasyMove.tDistance ? true : false;
    }
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
    }
}
