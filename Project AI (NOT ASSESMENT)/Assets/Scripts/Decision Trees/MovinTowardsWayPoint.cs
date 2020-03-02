using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovinTowardsWayPoint : IDecision
{
    public override bool CheckBool()
    {
        return false;
    }
    public override void MakeDecision()
    {
        LittleEasyMove.tPlayer.position = Vector3.MoveTowards(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position, 3 * Time.deltaTime);
    }
}
