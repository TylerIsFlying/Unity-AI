using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behaviors/Example")]
public class Behavior : ScriptableObject
{
    //public virtual bool Execute() { return false; } // executes the code and return true or false based if it failed or success
    public virtual bool Execute(GameObject p) { return false; } // executes the code and return true or false based if it failed or success
    public virtual bool Execute(GameObject p, GameObject t) { return false; } // executes the code and return true or false based if it failed or success
    public virtual bool Execute(GameObject p, List<string> tags) { return false; } // executes the code and return true or false based if it failed or success
}
