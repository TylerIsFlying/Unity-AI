using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Node
{
    public int g_score;
    public float f_score;
    public float h_score;
    public Vector3 position;
    public List<A_Node> connections = new List<A_Node>();
    public A_Node pastNode = null;
    public A_Node(int g_score,Vector3 position)
    {
        this.g_score = g_score;
        this.position = position;
    }
    public void CalF()
    {
        f_score = g_score + h_score;
    }
}
