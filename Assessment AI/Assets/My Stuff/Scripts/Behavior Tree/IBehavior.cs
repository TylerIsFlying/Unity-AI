using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IBehavior : ScriptableObject
{
    //public virtual bool Execute() { return false; } // executes the code and return true or false based if it failed or success
    public virtual bool Execute(GameObject p) { return false; } // executes the code and return true or false based if it failed or success
    public virtual bool Execute(GameObject p, GameObject t) { return false; } // executes the code and return true or false based if it failed or success
}
