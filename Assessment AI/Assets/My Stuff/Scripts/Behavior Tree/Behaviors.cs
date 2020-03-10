using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviors : MonoBehaviour
{
    [HideInInspector]
    public BehaviorScript behaviorStarter = null;
    [HideInInspector]
    public BehaviorScript currentBehavior = null;
    [HideInInspector]
    public Animal animal;
    public void Start()
    {
        animal = this.GetComponent<Animal>();
    }
}
