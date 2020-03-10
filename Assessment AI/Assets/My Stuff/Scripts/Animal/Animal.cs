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
    public float seekWater = 3f;
    public float seekFood = 3f;
    public string waterTag;
    public string foodTag;
    public float rangeToStop = 0.5f;
    public float resetWhenStopped = 4f;
    [HideInInspector]
    public int water;
    [HideInInspector]
    public int food;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public GameObject animalPoint;
    [Header("Timer Settings")]
    public float timePerLoss = 1f;
    private float timer = 0.0f;
    public List<Node> path = new List<Node>();
    private int pathAdder = 0;
    [HideInInspector]
    public bool reachedPoint = false;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public int id;
    Vector3 lastPos;
    Vector3 startPos;
    float timerStop;
    void Start()
    {
        startPos = transform.position;
        id = Random.Range(0, 99999);
        timer = timePerLoss;
        timerStop = resetWhenStopped;
        water = maxWater;
        food = maxFood;
        health = maxHealth;
        animalPoint = new GameObject("point");
        animalPoint.transform.parent = gameObject.transform;
        if (gameObject.GetComponent<Pathing>() != null) instance = gameObject.GetComponent<Pathing>();
    }
    void Update()
    {
        timer -= Time.deltaTime;
        timerStop -= Time.deltaTime;
        if (timer <= 0)
        {
            water--;
            food--;
            timer = timePerLoss;
        }
        if(timerStop <= 0)
        {
            if (transform.position == lastPos)
            {
                transform.position = startPos;
            }
            timerStop = resetWhenStopped;
            lastPos = transform.position;
        }
    }
    // Gets a food target
    public GameObject GetFoodTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(foodTag);
        GameObject target = targets.Length > 0 ? targets[0]:null;
        return target;
    }
    // Gets a water target
    public GameObject GetWaterTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(waterTag);
        GameObject target = targets.Length > 0 ? targets[0] : null;
        return target;
    }
    // clears the path
    public void ClearPath()
    {
        path.Clear();
    }
    public bool PathFound()
    {
        if (path.Count > 1)
            return true;
        else
            return false;
    }
    //returns water
    public float GetWater()
    {
        return water;
    }
    // returns food
    public float GetFood()
    {
        return food;
    }
    // gets the seekwater varible
    public float SeekWater()
    {
        return seekWater;
    }
    // gets the seekfood varible
    public float SeekFood()
    {
        return seekFood;
    }
    public void FindPath(GameObject target)
    {
        path.Clear();
        if (instance != null) instance.SetTarget(gameObject, target,path);
        pathAdder = 0;
    }
    public void FindPath(GameObject target, List<string> tags)
    {
        path.Clear();
        if (instance != null) instance.SetTarget(gameObject, target,tags,path);
        pathAdder = 0;
    }
    public bool MoveTowards()
    {
        if(path != null && path.Count > 0)
        {
            if (pathAdder < path.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, path[pathAdder].position, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, path[pathAdder].position) <= rangeToStop) pathAdder++;
            }
        }
        if (pathAdder >= path.Count) return true;
        else return false;
    }
    private void OnDrawGizmos()
    {
        for(int i = 0; i < path.Count; i++)
        {
            Gizmos.DrawWireCube(path[i].position, new Vector3(1, 1, 1));
        }
    }
}
