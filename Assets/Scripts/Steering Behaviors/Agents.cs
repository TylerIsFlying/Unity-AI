using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agents : MonoBehaviour
{
    [HideInInspector]
    public Vector3 velocity;
    public float maxVelocity;
    public float speed;
    public float mass;
    private Vector3 steeringForce;
    private bool freeze;
    void Start()
    {
        steeringForce = Vector3.zero;
        velocity = Vector3.zero;
        freeze = false;
    }

    void Update()
    {
        if (!freeze)
        {
            Vector3 steering = Vector3.ClampMagnitude(steeringForce, maxVelocity);
            steeringForce = Vector3.zero;
            steering /= mass;
            velocity = Vector3.ClampMagnitude(velocity + steering, speed);
            transform.position += velocity * Time.deltaTime;
            if (velocity != Vector3.zero)
            {
                transform.forward = velocity.normalized;
            }
        }
    }
    public void Steer(Vector3 steering)
    {
        steeringForce += steering;
    }
    public void SetFreeze()
    {
        freeze = true;
    }
    public void RemoveFreeze()
    {
        freeze = false;
    }
}
