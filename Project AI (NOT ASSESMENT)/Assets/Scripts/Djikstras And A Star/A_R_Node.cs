using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_R_Node
{
    public float g;
    public float f;
    public float h;
    public List<A_R_Node> connections = new List<A_R_Node>();
    public Vector3 position;
    public A_R_Node pastNode = null;
    public bool ignore = false;
    public A_R_Node(float g,Vector3 position)
    {
        this.g = g;
        this.position = position;
    }
    public void CalF()
    {
        f = g + h;
    }
}
