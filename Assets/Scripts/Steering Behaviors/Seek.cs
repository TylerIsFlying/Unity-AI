using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public Transform target; // target for the ai to steer towards
    Agent ag;
    void Start()
    {
        ag = GetComponent<Agent>();
    }

    void Update()
    {
        if(target != null)
        {
            Vector3 v = (target.position - transform.position).normalized * ag.maxVelocity;
            Vector3 force = v - ag.velocity;
            ag.Steer(force);
        }
    }
}
