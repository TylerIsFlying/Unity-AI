using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : MonoBehaviour
{
    public Transform target; // target for the ai to steer towards
    Agents ag;
    Agents targetAgent;
    void Start()
    {
        ag = GetComponent<Agents>();
        targetAgent = target.gameObject.GetComponent<Agents>();
    }

    void Update()
    {
        Vector3 v = (target.position + targetAgent.velocity) - transform.position;
        Vector3 force = (v.normalized * ag.maxVelocity) - ag.velocity;
        ag.Steer(force);
    }
}
