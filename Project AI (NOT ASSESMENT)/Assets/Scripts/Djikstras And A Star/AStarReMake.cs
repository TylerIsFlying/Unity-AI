using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarReMake : MonoBehaviour
{
    public List<string> tags;
    public Camera camera;
    public float speed = 3;
    public Vector3 size;
    public Vector3 boxSize;
    public GameObject target;
    public float startValue = 0;
    public float endValue = 0;
    public float avoidValue = 30;
    public float baseValue = 1;
    private List<A_R_Node> nodes = new List<A_R_Node>();
    private A_R_Node startNode = null;
    private A_R_Node endNode = null;
    private List<A_R_Node> path = new List<A_R_Node>();
    private List<A_R_Node> open = new List<A_R_Node>();
    private List<A_R_Node> closed = new List<A_R_Node>();
    private int pathAdder = -1;
    void Start()
    {
        CreateGrid(); // only creates the grid once
    }
    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                target.transform.position = new Vector3(hit.point.x, size.y, hit.point.z);
                CreatePath();
                pathAdder = path.Count - 1;
            }
        }
        if(path.Count > 0 && pathAdder >= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[pathAdder].position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position,path[pathAdder].position) <= 0.5f)
            {
                pathAdder--;
            }
        }
    }
    // Creates a grid of the nodes
    void CreateGrid()
    {
        for(float i = -size.x; i < size.x; i++)
        {
            for (float j = -size.z; j < size.z; j++)
            {
                nodes.Add(new A_R_Node(baseValue,new Vector3(i,size.y,j)));
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
            A_R_Node tmp = FindNode(new Vector3(nodes[i].position.x, nodes[i].position.y, nodes[i].position.z - 1));
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
    // Sets everything up
    void CreatePath()
    {
        FindPath();
    }
    // Finds the node
    A_R_Node FindNode(Vector3 value)
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].position == value)
                return nodes[i];
        }
        return null;
    }
    // gets the nearest node towards the player position
    A_R_Node GetNearestNode(Vector3 position)
    {
        A_R_Node node = null;
        float dst = 0;
        if (nodes.Count > 0)
        {
            dst = Vector3.Distance(position, nodes[0].position);
            node = nodes[0];
        }
        for(int i = 0; i < nodes.Count; i++)
        {
            if(Vector3.Distance(position,nodes[i].position) < dst)
            {
                node = nodes[i];
                dst = Vector3.Distance(position, nodes[i].position);
            }
        }
        return node;
    }
    // Will assign the values in the grid
    void AssignValues()
    {
        Collider[] colliders;
        endNode = null;
        startNode = null;
        for(int i = 0; i < nodes.Count; i++)
        {
            colliders = Physics.OverlapBox(nodes[i].position, boxSize/2);
            nodes[i].g = baseValue;
            nodes[i].ignore = false;
            nodes[i].pastNode = null;

            for (int j = 0; j < colliders.Length; j++)
            {
                for (int k = 0; k < tags.Count; k++)
                {
                    if (colliders[j].gameObject.CompareTag(tags[k]))
                    {
                        nodes[i].g = avoidValue;
                        nodes[i].ignore = true;
                    }
                }
            }
        }
        startNode = GetNearestNode(transform.position);
        endNode = GetNearestNode(target.transform.position);
        transform.position = startNode.position;
        target.transform.position = endNode.position;
        endNode.g = endValue;
        startNode.g = startValue;
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].h = (nodes[i].position.x + endNode.position.x) + (nodes[i].position.z + endNode.position.z);
        }
    }
    bool isClosesd(A_R_Node value)
    {
        for (int i = 0; i < closed.Count; i++)
        {
            if (closed[i] == value)
                return true;
        }
        return false;
    }
    bool isOpen(A_R_Node value)
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
        A_R_Node currentNode = startNode;
        bool found = false;
        while(!found)
        {
            if (!isClosesd(currentNode))
                closed.Add(currentNode);
            for(int i = 0; i < currentNode.connections.Count; i++)
            {
                if (!isOpen(currentNode.connections[i]) && !isClosesd(currentNode.connections[i]))
                {
                    if (!currentNode.connections[i].ignore)
                    {
                        currentNode.connections[i].g += currentNode.g;
                        if (currentNode.connections[i].pastNode == null)
                            currentNode.connections[i].pastNode = currentNode;
                        open.Add(currentNode.connections[i]);
                    }
                }
            }
            SortNodes(open);
            if (open.Count > 0)
            {
                currentNode = open[0];
                open.Remove(currentNode);
            }
            if(currentNode == endNode)
            {
                found = true;
            }
        }
    }
    void SortNodes(List<A_R_Node> items)
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
                            A_R_Node tmp = items[i + 1];
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
        closed.Clear();
        open.Clear();
        path.Clear();
        AssignValues();
        if(!startNode.ignore && !endNode.ignore)
        {
            CalNodes();
            A_R_Node currentNode = endNode;
            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.pastNode;
            }
        }
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
            for (int i  = 0; i < nodes.Count; i++)
            {
                if (nodes[i].ignore)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(nodes[i].position, boxSize);
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(nodes[i].position, boxSize);
                }
            }
            for(int i = 0; i < path.Count; i++)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(path[i].position,0.3f);
            }
        }
        else
        {
            for (float i = -size.x; i < size.x; i++)
            {
                for (float j = -size.z; j < size.z; j++)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(new Vector3(i, size.y, j), boxSize);
                }
            }
        }
    }
}
