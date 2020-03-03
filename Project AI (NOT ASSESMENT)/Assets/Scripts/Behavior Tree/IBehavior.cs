using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IBehavior : ScriptableObject
{
    public virtual bool Execute() { return false; } // executes the code and return true or false based if it failed or success
}
