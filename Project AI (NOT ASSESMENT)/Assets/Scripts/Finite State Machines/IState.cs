using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IState : ScriptableObject
{
    public virtual void Execute() { } // will keep running till it is on a new node
    public virtual void OnEnter() {} // will only do this once on the node when it enters it
    public virtual void OnExit() {} // will only do this when the node exits
    public virtual bool OnCondition() { return false; } // this is a condition used to determine what is the next one to go to 
}
