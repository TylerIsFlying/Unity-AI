using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [Header("Main Settings")]
    public float speed = 4f;
    [HideInInspector]
    public Pathing instance = null;
    public int maxHealth = 10;
    public int maxFood = 6;
    public int maxWater = 5;
    public float rangeToStop = 0.5f;
    [HideInInspector]
    public int water;
    [HideInInspector]
    public int food;
    [HideInInspector]
    public int health;
    [Header("Timer Settings")]
    public float timePerLoss = 0.1f;
    private float timer = 0.0f;
    public List<Node> path = new List<Node>();
    private int pathAdder = 0;
    void Start()
    {
        timer = timePerLoss;
        if (Pathing.GetInstance() != null) instance = Pathing.GetInstance();
    }
    public void FindPath(GameObject target)
    {
        if (instance != null) path = instance.SetTarget(gameObject, target);
        pathAdder = 0;
    }
    public void MoveTowards()
    {
        if(path != null && path.Count > 0)
        {
            if (pathAdder < path.Count)
            {
                if (Vector3.Distance(transform.position, path[pathAdder].position) <= rangeToStop) pathAdder++;
                transform.position = Vector3.MoveTowards(transform.position, path[pathAdder].position, speed * Time.deltaTime);
            }
        }
    }
    void Update()
    {
        if(instance != null && Pathing.GetInstance() != null) instance = Pathing.GetInstance();
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            water--;
            food--;
            timer = timePerLoss;
        }
    }
}
