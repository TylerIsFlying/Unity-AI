using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Node
{
    public int g_score;
    public Vector3 position;
    public List<D_Node> connections = new List<D_Node>();
    public D_Node pastNode = null;
    public D_Node(int g_score, Vector3 position)
    {
        this.g_score = g_score;
        this.position = position;
    }

}
