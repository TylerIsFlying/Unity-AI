using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public Camera camera;
    public Vector3 size;
    public Vector3 sizeOfBox;
    public List<string> tags;
    public GameObject target;
    public bool areaVisable = true;
    public bool showConnections = true;
    public bool showPastNodes = true;
    private A_Node targetNode = null;
    private A_Node startNode = null;
    private List<A_Node> nodes = new List<A_Node>();
    private List<A_Node> openList = new List<A_Node>();
    private List<A_Node> closedList = new List<A_Node>();
    private List<A_Node> path = new List<A_Node>();
    private A_Node currentNode = null;
    public bool showPath = true;
    public float speed = 1.5f;
    private int nextPath;


    void Start()
    {
        CreatePath();
    }
    void CreatePath()
    {
        targetNode = null;
        startNode = null;
        nodes.Clear();
        openList.Clear();
        closedList.Clear();
        path.Clear();
        currentNode = null;
        MakeNodes();
        LookForPath();
        nextPath = path.Count - 1;
    }
    void Update()
    {
        // to do
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                target.transform.position = hit.point;
                CreatePath();
            }
        }
        if (nextPath >= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[nextPath].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path[nextPath].position) < 0.5f)
                nextPath--;
        }
    }
    void SortNodes(List<A_Node> items)
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
                        items[i + 1].CalF();
                        if (items[i + 1].f_score < items[i].f_score)
                        {
                            A_Node tmp = items[i + 1];
                            items[i + 1] = items[i];
                            items[i] = tmp;
                        }
                    }
                }
                lUnsorted--;
            }
        }
    }
    bool isInClosesd(A_Node value)
    {
        for (int i = 0; i < closedList.Count; i++)
        {
            if (closedList[i] == value)
                return true;
        }
        return false;
    }
    bool isInOpen(A_Node value)
    {
        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i] == value)
                return true;
        }
        return false;
    }
    void CheckNodes(A_Node a, A_Node b)
    {
    }
    void LookForPath()
    {
        // do some sort of for loop in here
        currentNode = startNode;
        bool pathFound = false;
        int operations = 0;
        while (!pathFound)
        {

            if (!isInClosesd(currentNode))
                closedList.Add(currentNode);
            for (int i = 0; i < currentNode.connections.Count; i++)
            {
                if (!isInOpen(currentNode.connections[i]) && !isInClosesd(currentNode.connections[i]))
                {
                    currentNode.connections[i].g_score += currentNode.g_score;
                    if (currentNode.connections[i].pastNode == null)
                        currentNode.connections[i].pastNode = currentNode;
                    openList.Add(currentNode.connections[i]);
                }
            }
            SortNodes(openList);
            if (openList.Count > 0)
            {
                currentNode = openList[0];
                openList.Remove(currentNode);
            }
            if (currentNode == targetNode)
            {
                pathFound = true;
            }
            operations++;
        }
        Debug.Log(operations);
        pathFound = false;
        while (!pathFound)
        {
            if (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.pastNode;
            }
            else
            {
                pathFound = true;
            }
        }
    }
    void MakeNodes()
    {
        Collider[] coll;
        bool waitForNext = false;
        bool doPlayerOnce = false;
        bool doTargetOnce = false;
        Vector3 lastNodePos = Vector3.zero;
        int currNode = 0;
        for (float i = -size.x; i < size.x; i++)
        {
            for (float j = -size.z; j < size.z; j++)
            {
                coll = Physics.OverlapBox(new Vector3(i, size.y, j), sizeOfBox/2);
                foreach (Collider col in coll)
                {
                    foreach (string tag in tags)
                    {
                        if (col.gameObject.CompareTag(tag) && !waitForNext)
                        {
                            nodes.Add(new A_Node(2, new Vector3(i, size.y, j)));
                            waitForNext = true;
                        }
                    }
                    if (col.gameObject.CompareTag(this.gameObject.tag) && !waitForNext)
                    {
                        if (!doPlayerOnce)
                        {
                            nodes.Add(new A_Node(0, new Vector3(i, size.y, j)));
                            startNode = nodes[currNode];
                            waitForNext = true;
                            doPlayerOnce = true;
                        }
                    }
                    if (col.gameObject.CompareTag(target.gameObject.tag) && !waitForNext)
                    {
                        if (!doTargetOnce)
                        {
                            nodes.Add(new A_Node(-1, new Vector3(i, size.y, j)));
                            targetNode = nodes[currNode];
                            waitForNext = true;
                            doTargetOnce = true;
                        }
                    }
                }
                if (!waitForNext)
                {
                    nodes.Add(new A_Node(1, new Vector3(i, size.y, j)));
                }
                currNode++;
                waitForNext = false;
            }
        }
        foreach(A_Node node in nodes)
        {
            node.h_score = (node.position.x + targetNode.position.x) + (node.position.z + targetNode.position.z);
        }
        ConnectNodes();
    }
    A_Node SearchForNode(Vector3 value)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].position == value)
                return nodes[i];
        }
        return null;
    }
    void ConnectNodes()
    {
        int nodeCounter = 0;
        foreach (A_Node node in nodes)
        {
            A_Node tmp = SearchForNode(new Vector3(node.position.x, node.position.y, node.position.z - 1));
            if (tmp != null)
                node.connections.Add(tmp);
            tmp = SearchForNode(new Vector3(node.position.x, node.position.y, node.position.z + 1));
            if (tmp != null)
                node.connections.Add(tmp);
            tmp = SearchForNode(new Vector3(node.position.x - 1, node.position.y, node.position.z));
            if (tmp != null)
                node.connections.Add(tmp);
            tmp = SearchForNode(new Vector3(node.position.x + 1, node.position.y, node.position.z));
            if (tmp != null)
                node.connections.Add(tmp);
            nodeCounter++;
        }
    }
    void RemoveNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes.Remove(nodes[i]);
        }
    }
    private void OnDrawGizmos()
    {
        if (areaVisable)
        {
            if (Application.isPlaying)
            {
                int nodeCounter = 0;
                foreach (A_Node node in nodes)
                {
                    switch (node.g_score)
                    {
                        case 0:
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawWireCube(new Vector3(node.position.x, node.position.y, node.position.z), sizeOfBox);
                            break;
                        case 1:
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireCube(new Vector3(node.position.x, node.position.y, node.position.z), sizeOfBox);
                            break;
                        case 2:
                            Gizmos.color = Color.red;
                            Gizmos.DrawWireCube(new Vector3(node.position.x, node.position.y, node.position.z), sizeOfBox);
                            break;
                        case -1:
                            Gizmos.color = Color.magenta;
                            Gizmos.DrawWireCube(new Vector3(node.position.x, node.position.y, node.position.z), sizeOfBox);
                            break;
                    }
                    if (showConnections)
                    {
                        foreach (A_Node connection in node.connections)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawLine(node.position, connection.position);
                        }
                    }
                    if (showPastNodes)
                    {
                        if (node.pastNode != null)
                        {
                            Gizmos.color = Color.blue;
                            Gizmos.DrawLine(node.position, node.pastNode.position);
                        }
                    }
                    nodeCounter++;
                }
                for (int i = path.Count - 1; i >= 0; i--)
                {
                    if (showPath)
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawWireSphere(path[i].position, 0.3f);
                    }
                }
            }
            else
            {
                for (float i = -size.x; i < size.x; i++)
                {
                    Gizmos.color = Color.yellow;
                    for (float j = -size.z; j < size.z; j++)
                    {
                        Gizmos.DrawWireCube(new Vector3(i, size.y, j), sizeOfBox);
                    }
                }
            }
        }
    }
}
