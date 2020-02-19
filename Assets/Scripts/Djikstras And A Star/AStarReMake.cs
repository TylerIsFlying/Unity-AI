using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarReMake : MonoBehaviour
{
    public List<string> tags;
    public Vector3 size;
    public Vector3 boxSize;
    public GameObject target;
    public float startValue = 0;
    public float endValue = 0;
    public float avoidValue = 2;
    public float baseValue = 1;
    private List<Node> nodes = new List<Node>();
    private Node startNode = null;
    private Node endNode = null;
    private List<Node> path = new List<Node>();
    private List<Node> open = new List<Node>();
    private List<Node> closed = new List<Node>();
    void Start()
    {
        CreateGrid(); // only creates the grid once
        FindPath();
    }
    void Update()
    {
        
    }
    // Creates a grid of the nodes
    void CreateGrid()
    {
        for(float i = -size.x; i < size.x; i++)
        {
            for (float j = -size.z; j < size.z; j++)
            {
                nodes.Add(new Node(baseValue,new Vector3(i,size.y,j)));
            }
        }
        CreateConnections();
    }
    // Creates the connections for our grid
    void CreateConnections()
    {
        int nodeCounter = 0;
        for(int i = 0; i < nodes.Count; i++)
        {
            Node tmp = FindNode(new Vector3(nodes[i].position.x, nodes[i].position.y, nodes[i].position.z - 1));
            if (tmp != null)
                nodes[i].connections.Add(tmp);
            tmp = FindNode(new Vector3(nodes[i].position.x, nodes[i].position.y, nodes[i].position.z + 1));
            if (tmp != null)
                nodes[i].connections.Add(tmp);
            tmp = FindNode(new Vector3(nodes[i].position.x - 1, nodes[i].position.y, nodes[i].position.z));
            if (tmp != null)
                nodes[i].connections.Add(tmp);
            tmp = FindNode(new Vector3(nodes[i].position.x + 1, nodes[i].position.y, nodes[i].position.z));
            if (tmp != null)
                nodes[i].connections.Add(tmp);
            nodeCounter++;
        }
    }
    // Finds the node
    Node FindNode(Vector3 value)
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].position == value)
                return nodes[i];
        }
        return null;
    }
    // Will assign the values in the grid
    void AssignValues()
    {
        Collider[] colliders;
        bool startLocFound = false;
        bool endLocFound = false;
        for(int i = 0; i < nodes.Count; i++)
        {
            colliders = Physics.OverlapBox(nodes[i].position, boxSize);
            nodes[i].g = baseValue;
            for (int j = 0; j < colliders.Length; j++)
            {
                for (int k = 0; k < tags.Count; k++)
                {
                    if (colliders[j].gameObject.CompareTag(tags[k]))
                    {
                        nodes[i].g = avoidValue;
                    }
                }
                if (colliders[j].CompareTag(gameObject.tag) && !startLocFound)
                {
                    nodes[i].g = startValue;
                    startNode = nodes[i];
                    transform.position = startNode.position;
                    startLocFound = true;
                }
                else if (colliders[j].CompareTag(target.gameObject.tag) && !endLocFound)
                {
                    nodes[i].g = endValue;
                    endNode = nodes[i];
                    target.transform.position = endNode.position;
                    endLocFound = true;
                }
            }
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].h = (nodes[i].position.x + endNode.position.x) + (nodes[i].position.z + endNode.position.z);
        }
    }
    bool isClosesd(Node value)
    {
        for (int i = 0; i < closed.Count; i++)
        {
            if (closed[i] == value)
                return true;
        }
        return false;
    }
    bool isOpen(Node value)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (open[i] == value)
                return true;
        }
        return false;
    }
    // Calculate all the nodes
    void CalNodes()
    {
        Node currentNode = startNode;
        bool found = false;
        while(!found)
        {
            if (!isClosesd(currentNode))
                closed.Add(currentNode);
            for(int i = 0; i < currentNode.connections.Count; i++)
            {
                if (!isOpen(currentNode.connections[i]) && !isClosesd(currentNode.connections[i]))
                {
                    currentNode.connections[i].g += currentNode.g;
                    if (currentNode.connections[i].pastNode == null)
                        currentNode.connections[i].pastNode = currentNode;
                    open.Add(currentNode.connections[i]);
                    Debug.Log(currentNode.connections[i].g);
                }
            }
            SortNodes(open);
            if (open.Count > 0)
            {
                open[0] = currentNode;
                open.Remove(currentNode);
            }
            if(currentNode == endNode)
            {
                found = true;
            }
        }

    }
    void SortNodes(List<Node> items)
    {
        if (items.Count > 0)
        {
            bool isSorted = false;
            int lUnsorted = items.Count;
            while (!isSorted)
            {
                isSorted = true;
                for (int i = 0; i < lUnsorted; i++)
                {
                    if (i + 1 < items.Count)
                    {
                        items[i].CalF();
                        if (items[i + 1].f < items[i].f)
                        {
                            Node tmp = items[i + 1];
                            items[i + 1] = items[i];
                            items[i] = tmp;
                        }
                    }
                }
                lUnsorted--;
            }
        }
    }
    void FindPath()
    {
        AssignValues();
        CalNodes();
        Node currentNode = endNode;
        //while (currentNode != null)
        //{
        //    path.Add(currentNode);
        //    currentNode = currentNode.pastNode;
        //}
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            //for(int i = 0; i < nodes.Count; i++)
            //{
            //    Gizmos.color = Color.yellow;
            //    Gizmos.DrawWireCube(nodes[i].position, boxSize);
            //    for (int j = 0; j < nodes[i].connections.Count; j++)
            //    {
            //        Gizmos.color = Color.red;
            //        Gizmos.DrawLine(nodes[i].position, nodes[i].connections[j].position);
            //    }
            //}
            for(int i = 0; i < open.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(nodes[i].position, boxSize);
            }
            for (int i = 0; i < closed.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(nodes[i].position, boxSize);
            }
            for (int i = 0; i < path.Count; i++)
            {
               Gizmos.color = Color.red;
                Gizmos.DrawWireCube(path[i].position, boxSize);
            }
        }
        for (float i = -size.x; i < size.x; i++)
        {
            for(float j = -size.z; j < size.z; j++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(i,size.y,j), boxSize);
            }
        }
    }
}
