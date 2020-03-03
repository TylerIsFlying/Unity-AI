using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour
{
    public BehaviorScript behaviorStarter;
    private BehaviorScript currentBehavior;
    void Update()
    {
        if (currentBehavior != null)
        {
            currentBehavior.GetChildren(out currentBehavior);
        }
        else
        {
            currentBehavior = behaviorStarter;
        }
    }
}
