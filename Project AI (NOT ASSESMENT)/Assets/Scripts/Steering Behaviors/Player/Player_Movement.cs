using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject camera;
    Vector3 offset;
    void Start()
    {
        offset = camera.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime);
    }
    void LateUpdate()
    {
        camera.transform.position = transform.position + offset;
    }
}
