using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakinANewPoint : IDecision
{
    public override bool CheckBool()
    {
        return false;
    }
    public override void MakeDecision()
    {
        LittleEasyMove.tTarget.position = new Vector3(Random.Range(LittleEasyMove.tMin.x, LittleEasyMove.tMax.x), LittleEasyMove.tPlayer.position.y,
            Random.Range(LittleEasyMove.tMin.z, LittleEasyMove.tMax.z));
    }
}
