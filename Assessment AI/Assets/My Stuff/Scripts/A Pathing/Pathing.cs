using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    // my cost and stuff
    public float g = 0f;
    public float h = 0f;
    public float f { get { return g + h; } }
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
}
public class Pathing : MonoBehaviour
{
    [Header("Main Settings")]
    public int baseValue = 1; // used for all nodes
    public int usedValue = 0; // used for both end and starting 
    public float range = 1f;
    [Header("Range Settings")]
    private List<Node> closed = new List<Node>();
    private List<Node> open = new List<Node>();
    private Node startNode = new Node();
    private Node endNode = new Node();
    private int unitApart = 1;
    private PathingManager manager;
    private bool isUsableAgain = true;
    private Node[,,] grid;
    void Start()
    {
        manager = PathingManager.GetInstance();
    }
    // You can set the target for the pathing and get the path for it.
    public List<Node> SetTarget(GameObject player, GameObject target, List<Node> path)
    {
        if (isUsableAgain)
        {
            if (grid != null) Array.Clear(grid,0,grid.Length);
            else
            {
                int mx = Mathf.RoundToInt(manager.worldSize.x);
                int my = Mathf.RoundToInt(manager.worldSize.y);
                int mz = Mathf.RoundToInt(manager.worldSize.z);
                grid = new Node[mx, my, mz];
            }
            Array.Copy(manager.grid,grid,manager.grid.Length);
            isUsableAgain = false;
            closed.Clear();
            open.Clear();
            path.Clear();
            AssignNodes(player, target);
            if (startNode != null && endNode != null)
            {
                if (!startNode.ignore && !endNode.ignore)
                {
                    // just checking if it calculated the path for it
                    if (CalPath())
                    {
                        Node currentNode = endNode;
                        Node t = new Node(new Vector3(player.transform.position.x, endNode.position.y, player.transform.position.z));
                        Node w = new Node(new Vector3(target.transform.position.x, endNode.position.y, target.transform.position.z));
                        path.Add(w);
                        int counter = 0;
                        while (currentNode != null)
                        {
                            path.Add(currentNode);
                            currentNode = currentNode.pastNode;
                            if (currentNode == endNode)
                                currentNode = null;
                            if (counter > grid.Length)
                            {
                                path.Clear();
                                break;
                            }
                            counter++;
                        }
                        if (path.Count > 0)
                        {
                            path.Add(t);
                            path.Reverse();
                        }
                        else
                        {
                            path.Add(new Node(player.transform.position));
                        }
                    }
                    else
                    {
                        path.Add(new Node(player.transform.position));
                    }
                }
            }
            isUsableAgain = true;
        }
        return path;
    }
    public List<Node> SetTarget(GameObject player, GameObject target, List<string> tags, List<Node> path)
    {
        if (isUsableAgain)
        {
            isUsableAgain = false;
            if (grid != null) Array.Clear(grid, 0, grid.Length);
            else
            {
                int mx = Mathf.RoundToInt(manager.worldSize.x);
                int my = Mathf.RoundToInt(manager.worldSize.y);
                int mz = Mathf.RoundToInt(manager.worldSize.z);
                grid = new Node[mx, my, mz];
            }
            if(manager.grid == null)
            {
                manager.Setup();
            }
            Array.Copy(manager.grid, grid, manager.grid.Length);
            closed.Clear();
            open.Clear();
            path.Clear();
            AssignNodes(player, target, tags);
            if (startNode != null && endNode != null)
            {
                if (!startNode.ignore && !endNode.ignore)
                {
                    if (CalPath())
                    {
                        Node currentNode = endNode;
                        Node t = new Node(new Vector3(player.transform.position.x, endNode.position.y, player.transform.position.z));
                        Node w = new Node(new Vector3(target.transform.position.x, endNode.position.y, target.transform.position.z));
                        path.Add(w);
                        int counter = 0;
                        while (currentNode != null)
                        {
                            path.Add(currentNode);
                            currentNode = currentNode.pastNode;
                            if (currentNode == endNode)
                                currentNode = null;
                            if (counter > grid.Length)
                            {
                                path.Clear();
                                break;
                            }
                            counter++;
                        }
                        if (path.Count > 0)
                        {
                            path.Add(t);
                            path.Reverse();
                        }
                        else
                        {
                            path.Add(new Node(player.transform.position));
                        }
                    }
                }
                else
                {
                    path.Clear();
                    path.Add(new Node(player.transform.position));
                }
            }
        }
        isUsableAgain = true;
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
        for (int y = 0; y < manager.worldSize.y; y++)
        {
            for (int x = 0; x < manager.worldSize.x; x++)
            {
                for (int z = 0; z < manager.worldSize.z; z++)
                {
                    grid[x, y, z].g = baseValue;
                    grid[x, y, z].ignore = false;
                    grid[x, y, z].pastNode = null;
                    if (startNode == null && Vector3.Distance(player.transform.position, grid[x, y, z].position) < range)
                    {
                        startNode = grid[x, y, z];
                        startNode.g = usedValue;
                    }
                    if (endNode == null && Vector3.Distance(target.transform.position, grid[x, y, z].position) < range)
                    {
                        endNode = grid[x, y, z];
                        endNode.g = usedValue;
                    }
                }
            }
        }
    }
    private void AssignNodes(GameObject player, GameObject target, List<string> tags)
    {
        startNode = null;
        endNode = null;
        Collider[] colliders;
        for (int y = 0; y < manager.worldSize.y; y++)
        {
            for (int x = 0; x < manager.worldSize.x; x++)
            {
                for (int z = 0; z < manager.worldSize.z; z++)
                {
                    colliders = Physics.OverlapBox(grid[x,y,z].position, manager.boxSize/2);
                    grid[x, y, z].g = baseValue;
                    grid[x, y, z].ignore = false;
                    grid[x, y, z].pastNode = null;
                    for (int j = 0; j < colliders.Length; j++)
                    {
                        if (tags != null)
                        {
                            for (int k = 0; k < tags.Count; k++)
                            {
                                if (colliders[j].gameObject.CompareTag(tags[k]))
                                {
                                    grid[x, y, z].ignore = true;
                                }
                            }
                        }
                    }
                    if (startNode == null && Vector3.Distance(player.transform.position, grid[x, y, z].position) < range)
                    {
                        startNode = grid[x, y, z];
                        startNode.g = usedValue;
                    }
                    if (endNode == null && Vector3.Distance(target.transform.position, grid[x, y, z].position) < range)
                    {
                        endNode = grid[x, y, z];
                        endNode.g = usedValue;
                    }
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
    private bool CalPath()
    {
        Node currentNode = startNode;
        bool found = false;
        int count = 0;
        while (!found)
        {
            if (!isClosesd(currentNode)) closed.Add(currentNode);
            for (int i = 0; i < currentNode.connections.Count; i++)
            {
                if (!isOpen(currentNode.connections[i]) && !isClosesd(currentNode.connections[i]))
                {
                    if (!currentNode.connections[i].ignore)
                    {
                        currentNode.connections[i].h = (currentNode.connections[i].position.x + endNode.position.x) + (currentNode.connections[i].position.z + endNode.position.z);
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
            if(count > grid.Length)
            {
                return false;
            }
            count++;
        }
        return true;
    }
}
