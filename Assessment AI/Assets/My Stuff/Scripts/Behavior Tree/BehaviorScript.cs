using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behavior")]
public class BehaviorScript : ScriptableObject
{
    // stuff to setup the script
    [Header("Script")]
    public MonoScript behaviorScript;
    [Header("Children")]
    public List<BehaviorScript> children = null;
    [HideInInspector]
    public IBehavior behavior;
    [HideInInspector]
    public BehaviorScript parent = null;
    public bool checkAll = false;
    private bool ignore = false;
    public void Start()
    {
    }
    // function will create a instance of the class for the behaviour
    private void CreateBehavior(BehaviorScript b)
    {
        if (b.behaviorScript != null && b.behaviorScript.GetClass().IsSubclassOf(typeof(IBehavior)))
        {
            b.behavior = (IBehavior)ScriptableObject.CreateInstance(b.behaviorScript.GetClass());
        }
    }
    // will get the children and will execute it's behaviour and set the child to the current behaviour if it's true
    public bool GetChildren(out BehaviorScript currentBehavior, GameObject o)
    {
        if (!checkAll)
        {
            foreach(BehaviorScript child in children)
            {
                if (child.behavior == null)
                    CreateBehavior(child);
                if (child.behavior != null && child.behavior.Execute(o))
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
                    if (child.behavior != null && !child.behavior.Execute(o))
                    {
                        child.ignore = true;
                        if (this.parent != null) currentBehavior = this.parent;
                        else currentBehavior = null;
                        return false;
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
