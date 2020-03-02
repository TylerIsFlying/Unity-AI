using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_Travel : MonoBehaviour
{
    public Camera camera;
    NavMeshAgent myAgent;
    public GameObject objectUsedForMouse;
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
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
                myAgent.SetDestination(hit.point);
                objectUsedForMouse.transform.position = hit.point;
            }
        }
    }
}
