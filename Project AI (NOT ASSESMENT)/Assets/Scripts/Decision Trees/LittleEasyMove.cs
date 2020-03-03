using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LittleEasyMove : MonoBehaviour
{
    public GameObject target;
    public float distance = 1;
    public Vector3 min;
    public Vector3 max;
    public float speed;
    public static Vector3 tMax;
    public static Vector3 tMin;
    public static Transform tTarget;
    public static Transform tPlayer;
    public static NiceGold tPlayerGold;
    public static NiceGold tTargetGold;
    public static float tDistance;
    public static float tSpeed;
    void Start()
    {
        tPlayer = gameObject.GetComponent<Transform>();
        tTarget = target.GetComponent<Transform>();
        tMin = min;
        tMax = max;
        tPlayerGold = gameObject.GetComponent<NiceGold>();
        tTargetGold = target.GetComponent<NiceGold>();
    }
    private void Update()
    {
        tDistance = distance;
        tSpeed = speed;
    }
}
