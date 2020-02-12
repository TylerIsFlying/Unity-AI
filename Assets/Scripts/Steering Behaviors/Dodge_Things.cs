using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge_Things : MonoBehaviour
{
    float rayCastLength;
    RaycastHit hit;
    public float disiredDistance; // will stop after it reaches that distance
    public int layer;
    Agent ag;
    void Start()
    {
        ag = GetComponent<Agent>();
    }

    void Update()
    {
        int layerMask = 1 << layer;
        if (Physics.Raycast(transform.position, transform.forward, out hit, disiredDistance,layerMask))
        {
            Vector3 v = (transform.position + ag.velocity) - hit.transform.position;
            Vector3 force = (v.normalized * ag.maxVelocity) - ag.velocity;
            ag.Steer(force);
        }
        /*
        if (Vector3.Distance(hit.transform.position, transform.position) < disiredDistance)
        {
            Vector3 v = (transform.position - hit.transform.position).normalized * ag.maxVelocity;
            Vector3 force = v - ag.velocity;
            ag.Steer(force);
        }
        */
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.forward * disiredDistance);
    }
}
