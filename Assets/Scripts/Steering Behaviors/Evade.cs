using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : MonoBehaviour
{
    public Transform target; // target for the ai to steer towards
    public float range;
    Agents ag;
    Agents targetAgent;
    void Start()
    {
        ag = GetComponent<Agents>();
        targetAgent = target.gameObject.GetComponent<Agents>();
    }

    void Update()
    {
        Vector3 v = (transform.position + ag.velocity) - target.position;
        Vector3 force = (v.normalized * ag.maxVelocity) - ag.velocity;
        ag.Steer(force);
    }
}
