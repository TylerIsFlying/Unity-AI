using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionPro : MonoBehaviour
{
    private IDecision currentNode = null;
    public static IDecision startNode = null;
    void Update()
    {
        if(startNode != null)
        {
            currentNode = startNode;
            while (currentNode != null)
            {
                currentNode.MakeDecision();
                currentNode = currentNode.GetNode();
            }
        }
    }
}
