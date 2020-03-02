using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrival : MonoBehaviour
{
    public Transform target;
    public float range;
    public float rangeToStop;
    Agents ag;
    void Start()
    {
        ag = GetComponent<Agents>();
    }

    void Update()
    {

        ag.velocity = Vector3.ClampMagnitude((target.position-transform.position)/range, ag.maxVelocity);
        if (Vector3.Distance(transform.position,target.position) <= rangeToStop)
        {
            ag.SetFreeze();
        }
        else
        {
            ag.RemoveFreeze();
        }
    }
}
