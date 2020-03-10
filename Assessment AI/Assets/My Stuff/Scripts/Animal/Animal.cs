using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AniSettings
{
    public GameObject o;
    public Renderer render;
    public Color color;
    public AniSettings(GameObject o, Renderer render, Color color)
    {
        this.o = o;
        this.render = render;
        this.color = color;
    }
}
public class Animal : MonoBehaviour
{
    [Header("Main Settings")]
    public float speed = 4f; // used for the speed of the animal
    [HideInInspector]
    public Pathing instance = null; // get an instance of pathing
    public int maxHealth = 10; // max health
    public int maxFood = 6; // max food
    public int maxWater = 5; // max water
    public float seekWater = 3f; // when to seek for the water
    public float seekFood = 3f; // when to seek for the food
    public string waterTag; // water taf
    public string foodTag; // food tag
    public float rangeToStop = 0.5f; // when to stop
    public float resetWhenStopped = 4f; // when to reset when you stop moving
    [HideInInspector]
    public int water; // your water
    [HideInInspector]
    public int food; // your food
    [HideInInspector]
    public int health; // your health
    [HideInInspector]
    public GameObject animalPoint; // used for wander when you want to get to a certain point
    [Header("Timer Settings")]
    public float timePerLoss = 1f; // time per you lose water,food
    private float timer = 0.0f; // used to time and reset it for time per loss
    public List<Node> path = new List<Node>(); // list of all the nodes for the path
    private int pathAdder = 0; // used to go to the next path when its time
    [HideInInspector]
    public bool reachedPoint = false; // when you reached the point towards water or food
    [HideInInspector]
    public GameObject target; // current target
    [HideInInspector]
    public int id; // id of animal
    Vector3 lastPos; // your last position
    Vector3 startPos; // your first position
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
    //when the path is found
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
    // used to find the path
    public void FindPath(GameObject target)
    {
        path.Clear();
        if (instance != null) instance.SetTarget(gameObject, target,path);
        pathAdder = 0;
    }
    // used to find the path and ignore the tags
    public void FindPath(GameObject target, List<string> tags)
    {
        path.Clear();
        if (instance != null) instance.SetTarget(gameObject, target,tags,path);
        pathAdder = 0;
    }
    // movetowards the next node
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
    // draws your nodes positions
    private void OnDrawGizmos()
    {
        for(int i = 0; i < path.Count; i++)
        {
            Gizmos.DrawWireCube(path[i].position, new Vector3(1, 1, 1));
        }
    }
}
