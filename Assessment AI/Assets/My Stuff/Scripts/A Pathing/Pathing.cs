using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // my cost and stuff
    public float g = 0f;
    public float h = 0f;
    public float f = 0f;
    // positon of the object
    public Vector3 position;
    // will set their orginal position
    public Vector3 orgPosition;
    // this is what my past node was
    public Node pastNode = null;
    // used for some more simple stuff
    public bool ignore = false;
    // list of connections
    public List<Node> connections = new List<Node>();
    public Node() { }
    public Node(Vector3 position)
    {
        this.position = position;
        this.orgPosition = position;
    }
    public void CalF()
    {
        f = g + h;
    }
}
public class Pathing : MonoBehaviour
{
    [Header("Main Settings")]
    public int baseValue = 1; // used for all nodes
    public int usedValue = 0; // used for both end and starting 
    public float range = 0.5f;
    public bool showArea = false;
    [Header("Range Settings")]
    public Vector3 min;
    public Vector3 max;
    public Vector3 boxSize;
    private List<Node> nodes = new List<Node>();
    private List<Node> closed = new List<Node>();
    private List<Node> open = new List<Node>();
    private List<Node> path = new List<Node>();
    private Node startNode = new Node();
    private Node endNode = new Node();
    private int unitApart = 1;
    private static Pathing instance;
    void Start()
    {
        instance = gameObject.GetComponent<Pathing>();
        CreateGrid();
        CreateConnections();
    }
    // gets an instance of it
    public static Pathing GetInstance()
    {
        if (instance != null) return instance;
        else return null;
    }
    // You can set the target for the pathing and get the path for it.
    public List<Node> SetTarget(GameObject player, GameObject target)
    {
        closed.Clear();
        open.Clear();
        path.Clear();
        AssignNodes(player,target);
        if(startNode != null && endNode != null)
        {
            if (!startNode.ignore && !endNode.ignore)
            {
                CalPath();
                Node currentNode = endNode;
                List<Node> tmp = new List<Node>();
                Node t = new Node(new Vector3(player.transform.position.x, endNode.position.y, player.transform.position.z));
                Node w = new Node(new Vector3(target.transform.position.x, endNode.position.y, target.transform.position.z));
                tmp.Add(w);
                while (currentNode != null)
                {
                    tmp.Add(currentNode);
                    currentNode = currentNode.pastNode;
                    if (currentNode == endNode)
                        currentNode = null;
                }
                tmp.Add(t);
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    path.Add(tmp[i]);
                }
            }
        }
        return path;
    }
    public List<Node> SetTarget(GameObject player, GameObject target, List<string> tags)
    {
        closed.Clear();
        open.Clear();
        path.Clear();
        AssignNodes(player, target, tags);
        if (startNode != null && endNode != null)
        {
            if (!startNode.ignore && !endNode.ignore)
            {
                CalPath();
                Node currentNode = endNode;
                List<Node> tmp = new List<Node>();
                Node t = new Node(new Vector3(player.transform.position.x, endNode.position.y, player.transform.position.z));
                Node w = new Node(new Vector3(target.transform.position.x, endNode.position.y, target.transform.position.z));
                tmp.Add(w);
                while (currentNode != null)
                {
                    tmp.Add(currentNode);
                    currentNode = currentNode.pastNode;
                    if (currentNode == endNode)
                        currentNode = null;
                }
                tmp.Add(t);
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    path.Add(tmp[i]);
                }
            }
        }
        return path;
    }
    // gets the distance
    private float GetDistance(Vector3 pos, Vector3 target)
    {
        return (pos - target).magnitude;
    }
    private void AssignNodes(GameObject player, GameObject target)
    {
        startNode = null;
        endNode = null;
        Collider[] colliders;
        for (int i = 0; i < nodes.Count; i++)
        {
            colliders = Physics.OverlapBox(nodes[i].position, boxSize / 2);
            nodes[i].g = baseValue;
            nodes[i].ignore = false;
            nodes[i].pastNode = null;

            for (int j = 0; j < colliders.Length; j++)
            {
                if(startNode == null && Vector3.Distance(player.transform.position,colliders[j].transform.position) < range)
                {
                    startNode = nodes[i];
                    startNode.g = usedValue;
                }
                if (endNode == null && Vector3.Distance(target.transform.position, colliders[j].transform.position) < range)
                {
                    endNode = nodes[i];
                    endNode.g = usedValue;
                }
            }
        }
        if (endNode != null && startNode != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].h = (nodes[i].position.x + endNode.position.x) + (nodes[i].position.z + endNode.position.z);
            }
        }

    }
    private void AssignNodes(GameObject player, GameObject target, List<string> tags)
    {
        startNode = null;
        endNode = null;
        Collider[] colliders;
        for (int i = 0; i < nodes.Count; i++)
        {
            colliders = Physics.OverlapBox(nodes[i].position, boxSize / 2);
            nodes[i].g = baseValue;
            nodes[i].ignore = false;
            nodes[i].pastNode = null;

            for (int j = 0; j < colliders.Length; j++)
            {
                if (tags != null)
                {
                    for (int k = 0; k < tags.Count; k++)
                    {
                        if (colliders[j].gameObject.CompareTag(tags[k]))
                        {
                            nodes[i].ignore = true;
                        }
                    }
                }
                if (startNode == null && Vector3.Distance(player.transform.position, colliders[j].transform.position) < range)
                {
                    startNode = nodes[i];
                    startNode.g = usedValue;
                }
                if (endNode == null && Vector3.Distance(target.transform.position, colliders[j].transform.position) < range)
                {
                    endNode = nodes[i];
                    endNode.g = usedValue;
                }
            }
        }
        if (endNode != null && startNode != null)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].h = (nodes[i].position.x + endNode.position.x) + (nodes[i].position.z + endNode.position.z);
            }
        }

    }
    // creates the connections
    private void CreateConnections()
    {
        int nodeCounter = 0;
        for (int i = 0; i < nodes.Count; i++)
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
    private Node FindNode(Vector3 value)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].position == value)
                return nodes[i];
        }
        return null;
    }
    // sets the grid up
    private void CreateGrid()
    {
        for(float y = min.y; y < max.y; y++)
        {
            for(float x = min.x; x < max.x; x++)
            {
                for(float z = min.z; z < max.z; z++)
                {
                    Node node = new Node(new Vector3(x,y,z));
                    nodes.Add(node);
                }
            }
        }
    }
    // returns if the value is in it
    private bool isClosesd(Node value)
    {
        for (int i = 0; i < closed.Count; i++)
        {
            if (closed[i] == value)
                return true;
        }
        return false;
    }
    // returns if the value is in it
    private bool isOpen(Node value)
    {
        for (int i = 0; i < open.Count; i++)
        {
            if (open[i] == value)
                return true;
        }
        return false;
    }
    // sorting nodes with higher f score
    private void SortNodes(List<Node> items)
    {
        if (items.Count > 0)
        {
            int j = 0;
            int keyValue = 0;
            for (int i = 0; i < items.Count; i++) items[i].CalF();
            for (int i = 0; i < items.Count; ++i)
            {
                keyValue = i;
                j = i - 1;
                while (j >= 0 && items[j].f > items[keyValue].f)
                {
                    Node tmp = items[j];
                    items[j] = items[keyValue];
                    items[keyValue] = tmp;
                    keyValue = j--;
                }
            }
        }
    }
    // calculate the path
    private void CalPath()
    {
        Node currentNode = startNode;
        bool found = false;
        while (!found)
        {
            if (!isClosesd(currentNode))
                closed.Add(currentNode);
            for (int i = 0; i < currentNode.connections.Count; i++)
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
            if (currentNode == endNode)
            {
                found = true;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (showArea)
        {
            for (float y = min.y; y < max.y; y++)
            {
                for (float x = min.x; x < max.x; x++)
                {
                    for (float z = min.z; z < max.z; z++)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireCube(new Vector3(x, y, z), boxSize);
                    }
                }
            }
        }
    }
}
