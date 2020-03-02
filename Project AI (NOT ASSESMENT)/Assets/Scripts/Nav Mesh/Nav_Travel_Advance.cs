using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_Travel_Advance : MonoBehaviour
{
    public Camera camera;
    public float speed;
    NavMeshAgent myAgent;
    NavMeshPath path;
    public GameObject objectUsedForMouse;
    int adder;
    Vector3 hits;
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        adder = 0;
        hits = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                NavMesh.CalculatePath(transform.position, hit.point, NavMesh.AllAreas, path);
                objectUsedForMouse.transform.position = hit.point;
                hits = hit.point;
                adder = 0;
            }
        }
        if (hits != Vector3.zero)
        {
            try
            {
                transform.position = Vector3.MoveTowards(transform.position,path.corners[adder],speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, path.corners[adder]) <= 1 && adder < path.corners.Length - 1)
                    adder++;
            }
            catch (System.IndexOutOfRangeException e)
            { }
        }
    }
    void OnDrawGizmos()
    {
        if(path != null)
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                if (Vector3.Distance(transform.position, path.corners[i]) <= 1)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(path.corners[i], new Vector3(1, 1, 1));
                }
                else
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(path.corners[i], new Vector3(1, 1, 1));
                }
            }
        }
    }
}
