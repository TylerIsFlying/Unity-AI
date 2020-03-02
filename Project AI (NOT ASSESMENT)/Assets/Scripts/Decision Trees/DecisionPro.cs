using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionPro : MonoBehaviour
{
    private IDecision currentNode = null;
    [HideInInspector]
    public IDecision startNode = null;
    private void Start()
    {
        startNode = new Sample_Decision();
        startNode.SetNode(new MovinTowardsWayPoint(), new MakinANewPoint());
    }
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
