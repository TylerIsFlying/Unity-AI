using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behavior")]
public class BehaviorScript : ScriptableObject
{
    public MonoScript behaviorScript;
    public List<BehaviorScript> children = null;
    [HideInInspector]
    public IBehavior behavior;
    [HideInInspector]
    public BehaviorScript parent = null;
    public bool checkAll = false;
    private bool ignore = false;
    private void CreateBehavior(BehaviorScript b)
    {
        if (b.behaviorScript != null && b.behaviorScript.GetClass().IsSubclassOf(typeof(IBehavior)))
        {
            b.behavior = (IBehavior)ScriptableObject.CreateInstance(b.behaviorScript.GetClass());
        }
    }
    public bool GetChildren(out BehaviorScript currentBehavior)
    {
        if (!checkAll)
        {
            foreach(BehaviorScript child in children)
            {
                if (child.behavior == null)
                    CreateBehavior(child);
                if (child.behavior != null && child.behavior.Execute())
                {
                    currentBehavior = child;
                    child.parent = this;
                    return true;
                }
            }
            if (this.parent != null) currentBehavior = this.parent;
            else currentBehavior = null;
            return false;
        }
        else
        {
            foreach (BehaviorScript child in children)
            {
                if (!child.ignore)
                {
                    if (child.behavior != null && !child.behavior.Execute())
                    {
                        child.ignore = true;
                        if (this.parent != null) currentBehavior = this.parent;
                        else currentBehavior = null;
                    }
                }
                child.ignore = false;
            }
            if (this.parent != null) currentBehavior = this.parent;
            else currentBehavior = null;
            return true;
        }
    }
}
