using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingManager : MonoBehaviour
{
    [HideInInspector]
    public Node[,,] grid;
    public Vector3 worldSize;
    public bool showArea = false;
    public Vector3 boxSize;
    private static PathingManager instance;
    void Start()
    {
        instance = gameObject.GetComponent<PathingManager>();
        Setup();
    }
    public void Setup()
    {
        int mx = Mathf.RoundToInt(worldSize.x);
        int my = Mathf.RoundToInt(worldSize.y);
        int mz = Mathf.RoundToInt(worldSize.z);
        grid = new Node[mx, my, mz];
        CreateGrid();
        CreateConnections();
    }
    public static PathingManager GetInstance()
    {
        if (instance != null) return instance;
        else return null;
    }
    // Sets the nodes connections
    private void SetNodeConnection(Node n, int x, int y, int z)
    {
        int lx, ly, lz;
        // doing x stuff
        lx = x + 1;
        if (lx >= 0 && lx < grid.GetLength(0)) n.connections.Add(grid[lx, y, z]);
        lx = x - 1;
        // doing y stuff
        if (lx >= 0 && lx < grid.GetLength(0)) n.connections.Add(grid[lx, y, z]);
        ly = y + 1;
        if (ly >= 0 && ly < grid.GetLength(1)) n.connections.Add(grid[x, ly, z]);
        ly = y - 1;
        if (ly >= 0 && ly < grid.GetLength(1)) n.connections.Add(grid[x, ly, z]);
        // doing z stuff
        lz = z + 1;
        if (lz >= 0 && lz < grid.GetLength(2)) n.connections.Add(grid[x, y, lz]);
        lz = z - 1;
        if (lz >= 0 && lz < grid.GetLength(2)) n.connections.Add(grid[x, y, lz]);
    }
    // creates the connections
    private void CreateConnections()
    {
        for (int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                for (int z = 0; z < worldSize.z; z++)
                {
                    SetNodeConnection(grid[x, y, z], x, y, z);
                }
            }
        }
    }
    // sets the grid up
    private void CreateGrid()
    {
        for (int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                for (int z = 0; z < worldSize.z; z++)
                {
                    grid[x, y, z] = new Node(new Vector3(x, y, z));
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (showArea)
        {
            for (float y = 0; y < worldSize.y; y++)
            {
                for (float x = 0; x < worldSize.x; x++)
                {
                    for (float z = 0; z < worldSize.z; z++)
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireCube(new Vector3(x, y, z), boxSize);
                    }
                }
            }
        }
        if (Application.isPlaying)
        {
            foreach (Node n in grid)
            {
                if (n.ignore)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(n.position, boxSize);
                }
            }
        }
    }
}
