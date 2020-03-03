using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public StateScript stateStart = null;
    private StateScript currState = null;
    private void Start()
    {
        stateStart.StartUp();
    }
    void Update()
    {
        if(stateStart != null)
        {
            if (currState == null) currState = stateStart;
            currState.Updating(out currState);
        }
    }
}
