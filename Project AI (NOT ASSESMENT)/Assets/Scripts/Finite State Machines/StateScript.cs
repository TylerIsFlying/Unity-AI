using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "State Machine", menuName = "State Machine/State")]
public class StateScript : ScriptableObject
{
    public int priority = 0; // zero is the biggest priority
    public bool globalMark = false; // mark this as a global
    public List<StateScript> children = null;
    public MonoScript script = null;
    private IState state = null;
    private bool entered = false;
    public void StartUp()
    {
        entered = false;
    }
    private void CreateScript(StateScript s)
    {
        if (s.script != null && s.script.GetClass().IsSubclassOf(typeof(IState)))
        {
            s.state = (IState)ScriptableObject.CreateInstance(s.script.GetClass());
        }
    }
    public void Sort()
    {
        int j = 0;
        int keyValue = 0;
        for (int i = 0; i < children.Count; ++i)
        {
            keyValue = i;
            j = i - 1;
            while (j >= 0 && children[j].priority > children[keyValue].priority)
            {
                StateScript tmp = children[j];
                children[j] = children[keyValue];
                children[keyValue] = tmp;
                keyValue = j--;
            }
        }
    }
    public void ResetValues()
    {
        entered = false;
    }
    public void Updating(out StateScript stateScript)
    {
        StateScript tmp = null;
        if(state != null)
        {
            if (!entered)
            {
                state.OnEnter();
                entered = true;
                Sort();
            }
            state.Execute();
            if (globalMark)
            {
                foreach (StateScript child in children)
                {
                    bool globalUsed = false;
                    foreach(StateScript c in child.children)
                    {
                        if (c == this)
                        {
                            globalUsed = true;
                            break;
                        }
                    }
                    if(!globalUsed) child.children.Add(this);
                }
            }
            foreach (StateScript child in children)
            {
                if(child.state == null)
                    CreateScript(child);
                if (child.state.OnCondition())
                {
                    entered = false;
                    tmp = child;
                    state.OnExit();
                    child.priority++;
                    if (child.priority > children.Count) child.priority = 0;
                    break;
                }
            }
        }else CreateScript(this);
        stateScript = tmp != null ? tmp:this;
    }
}
