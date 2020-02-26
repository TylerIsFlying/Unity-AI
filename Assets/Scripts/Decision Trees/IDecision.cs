using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct INode
{
    public IDecision falseNode;
    public IDecision trueNode;
}
public class IDecision : ScriptableObject
{
    public string ID = "";
    public int falseInt = 0;
    public int trueInt = 0;
    public bool starting = false;
    private INode node;
    public virtual void MakeDecision() {}
    public virtual bool CheckBool() { return false; } // returns true or false based on your action
    public void SetNode(IDecision fal, IDecision tru)
    {
        node.falseNode = fal;
        node.trueNode = tru;
    }
    public IDecision GetNode() // will get the newest node if it returns true or false
    {
        return this.CheckBool() ? node.trueNode : node.falseNode;
    }
    public INode GetINode()
    {
        return node;
    }
}
