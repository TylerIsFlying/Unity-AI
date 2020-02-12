using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public GameObject camera;
    Vector3 offset;
    void Start()
    {
        offset = camera.transform.position - transform.position;
    }

    void LateUpdate()
    {
        camera.transform.position = transform.position + offset;
    }
}
