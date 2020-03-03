using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsObj : IBehavior
{
    public override bool Execute()
    {
        LittleEasyMove.tPlayer.position = Vector3.MoveTowards(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position,
            LittleEasyMove.tSpeed * Time.deltaTime);
        if (Vector3.Distance(LittleEasyMove.tPlayer.position, LittleEasyMove.tTarget.position) <= LittleEasyMove.tDistance)
            return false;
        return true;
    }
}
