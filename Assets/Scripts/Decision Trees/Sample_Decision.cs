using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Decision : IDecision
{
    public override bool CheckBool()
    {
        return Vector3.Distance(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position) <= 1f ? true : false;
    }
    public override void MakeDecision()
    {
    }
}