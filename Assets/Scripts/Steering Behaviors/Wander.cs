using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    public float range;
    public float jitterRange;
    Vector3 target;
    Agent ag;
    Vector3 jitt;
    void Start()
    {
        ag = GetComponent<Agent>();
        SetTarget();
    }

    void Update()
    {
        Vector3 v = (target - transform.position).normalized * ag.maxVelocity;
        Vector3 force = v - ag.velocity;
        ag.Steer(force);
        if (Vector3.Distance(target, transform.position) < range)
        {
            //Debug.Log("Yes");
            SetTarget();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target, range);
        Gizmos.DrawCube(transform.forward + transform.position, new Vector3(1, 1, 1));
        Gizmos.color = Color.green;
    }
    void SetTarget()
    {
        Vector3 tPoint = Random.onUnitSphere * range;
        jitt = new Vector3(Random.Range(-jitterRange, jitterRange) + tPoint.x, transform.position.y-1, Random.Range(-jitterRange, jitterRange) + tPoint.z);
        target = (transform.position + transform.forward) + jitt;
    }
}
