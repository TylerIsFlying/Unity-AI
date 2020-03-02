using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBehavior
{
    bool Execute(GameObject o); // executes the code and return true or false based if it failed or success
}
