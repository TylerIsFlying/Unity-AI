using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleEasyMove : MonoBehaviour
{
    public GameObject target;
    public Vector3 min;
    public Vector3 max;
    public static Vector3 tMax;
    public static Vector3 tMin;
    public static Transform tTarget;
    public static Transform tPlayer;
    void Start()
    {
        tPlayer = this.gameObject.GetComponent<Transform>();
        tTarget = target.GetComponent<Transform>();
        tMin = min;
        tMax = max;
    }
}
