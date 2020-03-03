using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFarAway : IBehavior
{
    public override bool Execute()
    {
        return Vector3.Distance(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position) <= LittleEasyMove.tDistance ? false:true;
    }
}
