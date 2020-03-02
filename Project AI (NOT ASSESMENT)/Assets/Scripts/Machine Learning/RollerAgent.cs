using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensor;
using MLAgents.CommunicatorObjects;
using MLAgents.InferenceBrain;
public class RayCasterPro
{
    public float RayCast(List<string> tags, Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider col in colliders)
        {
            foreach(string tag in tags)
            {
                if (col.gameObject.CompareTag(tag))
                {
                    return Vector3.Distance(position, col.gameObject.transform.position);
                }
            }
        }
        return -1;
    }
}
public class RollerAgent : Agent
{
    public List<string> tags;
    public float radiusForArea = 3.0f;
    public Transform target;
    public Vector3 startPos;
    public Vector3 min;
    public Vector3 max;
    private RayCasterPro proLife = new RayCasterPro();
    private Rigidbody rb;
    [HideInInspector]
    public Vector3 movement = Vector3.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void CollectObservations()
    {
        AddVectorObs(target.position);
        AddVectorObs(transform.position);
        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.z);
        AddVectorObs(rb.angularVelocity.y);
    }
    public override void AgentAction(float[] act)
    {
        movement = new Vector3(act[0], transform.position.y, act[1]);
        if (transform.position.y <= 0)
            Done();
        if(Vector3.Distance(transform.position, target.position) <= 1f)
        {
            AddReward(1.0f);
            Done();
        }
    }
    public override void AgentReset()
    {
        transform.position = startPos;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        target.transform.position = new Vector3(Random.Range(min.x, max.x), target.transform.position.y, Random.Range(min.z, max.z));
    }
    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
