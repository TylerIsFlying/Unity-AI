using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    public Pathing instance;
    public GameObject target;
    private List<Node> path = null;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            path = instance.SetTarget(gameObject,target);
        }
    }
    private void OnDrawGizmos()
    {
        if(path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(path[i].position, new Vector3(1, 1, 1));
            }
        }
    }
}
