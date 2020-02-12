using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : MonoBehaviour
{
    public Transform target; // target for the ai to steer towards
    public float disiredDistance; // will stop after it reaches that distance
    public float weight;
    public AnimationCurve fleeCurve;
    Agent ag;
    void Start()
    {
        ag = GetComponent<Agent>();
    }

    void Update()
    {
        if(Vector3.Distance(target.position,transform.position) < disiredDistance)
        {
            Vector3 v = (transform.position - target.position).normalized * ag.maxVelocity;
            Vector3 force = v - ag.velocity;
            Vector3 vectorToMe = transform.position - target.position;
            float dist = vectorToMe.magnitude;
            weight = fleeCurve.Evaluate(Mathf.InverseLerp(disiredDistance, 0, dist));
            ag.Steer(force * weight);
        }
        else
        {
        }
    }
}
