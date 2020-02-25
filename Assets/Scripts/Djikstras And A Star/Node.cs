using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public float g;
    public float f;
    public float h;
    public List<Node> connections = new List<Node>();
    public Vector3 position;
    public Node pastNode = null;
    public bool ignore = false;
    public Node(float g,Vector3 position)
    {
        this.g = g;
        this.position = position;
    }
    public void CalF()
    {
        f = g + h;
    }
}
