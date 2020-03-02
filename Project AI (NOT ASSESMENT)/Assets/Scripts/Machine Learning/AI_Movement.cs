using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    RollerAgent rg;
    void Start()
    {
        rg = GetComponent<RollerAgent>();
    }

    void Update()
    {
        Vector3 target = rg.movement;
        transform.position = Vector3.MoveTowards(transform.position, target, 3 * Time.deltaTime);
    }
}
