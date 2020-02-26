using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behavior")]
public class BehaviorScript : ScriptableObject
{
    public MonoScript behaviorScript;
    public List<BehaviorScript> children = null;
    public IBehavior behavior;
    public bool checkAll = false;
    public BehaviorScript()
    {
        if (behaviorScript != null && behaviorScript.GetClass().IsSubclassOf(typeof(IBehavior)))
        {
            behavior = behaviorScript != null ? (IBehavior)ScriptableObject.CreateInstance(behaviorScript.GetClass()) : null;
        }
    }
    public bool GetChildren(GameObject o, out BehaviorScript currentBehavior)
    {
        if (checkAll)
        {
            foreach(BehaviorScript child in children)
            {
                if (child.behavior != null && child.behavior.Execute(o))
                {
                    currentBehavior = child;
                    return true;
                }
            }
            currentBehavior = null;
            return false;
        }
        else
        {
            foreach (BehaviorScript child in children)
            {
                if (!child.behavior.Execute(o))
                {
                    currentBehavior = null;
                    return false;
                }
                else
                {
                    currentBehavior = child;
                }
            }
            currentBehavior = null;
            return true;
        }
    }
}
