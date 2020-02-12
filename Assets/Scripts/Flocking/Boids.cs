using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    public int boidsAmount;
    public int distanceFromBoidsMin;
    public int distanceFromBoidsMax;
    public Vector3 position;
    public GameObject prefab;
    public bool areaVisiable = true; // can set to true or false for it to display
    public float time;
    List<GameObject> myBoids = new List<GameObject>();
    private int distanceFromBoids;
    private float timer;
    void Start()
    {
        CreateBoids();
        distanceFromBoids = Random.Range(distanceFromBoidsMin, distanceFromBoidsMax);
        timer = time;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            distanceFromBoids = Random.Range(distanceFromBoidsMin, distanceFromBoidsMax);
            timer = time;
        }
        MoveBoids();
    }
    // Crearting our boids
    public void CreateBoids()
    {
        for(int i = 0; i < boidsAmount; ++i)
        {
            myBoids.Add(Instantiate(prefab));
            myBoids[i].transform.position = new Vector3(Random.Range(-position.x, position.x), position.y, Random.Range(-position.z, position.z));
        }
    }
    public void MoveBoids()
    {
        Vector3 v1, v2, v3, v4;
        foreach(GameObject b in myBoids)
        {
            v1 = Rule1(b);
            v2 = Rule2(b);
            v3 = Rule3(b);
           // v4 = BoundPosition(b);
            b.GetComponent<Agent>().velocity = Vector3.ClampMagnitude(b.GetComponent<Agent>().velocity + v1 + v2 + v3, b.GetComponent<Agent>().maxVelocity);
            b.transform.position =  b.transform.position + b.GetComponent<Agent>().velocity;
            b.transform.position = BoundPosition(b);
        }
    }
    public Vector3 Rule1(GameObject a)
    {
        Vector3 pc = new Vector3();
        foreach(GameObject b in myBoids)
        {
            if(b != a)
            {
                pc = pc + b.transform.position;
            }
        }
        pc = (pc) / (boidsAmount - 1);
        return (pc - a.transform.position).normalized;
    }
    public Vector3 Rule2(GameObject a)
    {
        Vector3 c = Vector3.zero;
        foreach(GameObject b in myBoids)
        {
            if(b != a)
            {
                if(Vector3.Distance(b.transform.position, a.transform.position) < distanceFromBoids)
                {
                    c = c - (b.transform.position - a.transform.position);
                }
            }
        }
        return c;
    }
    public Vector3 Rule3(GameObject a)
    {
        Vector3 pv = Vector3.zero;
        foreach(GameObject b in myBoids)
        {
            if(b != a)
            {
                pv = pv + b.GetComponent<Agent>().velocity;
            }
        }
        pv = pv / (boidsAmount - 1);
        return (pv - a.GetComponent<Agent>().velocity).normalized; // do somethin latter ok :)
    }
    public void LimitVelocity(GameObject b)
    {

    }
    public Vector3 BoundPosition(GameObject b)
    {
        Vector3 v = b.transform.position;
        if (b.transform.position.x < -position.x)
            v.x = -position.x;
        else if (b.transform.position.x > position.x)
            v.x = position.x;
        if (b.transform.position.z < -position.z)
            v.z = -position.z;
        else if (b.transform.position.z > position.z)
            v.z = position.z;
        return v;
    }
    public void OnDrawGizmos()
    {
        if (areaVisiable)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(new Vector3(position.x, position.y, position.z), new Vector3(1, 1, 1));
            Gizmos.color = Color.green;
            Gizmos.DrawCube(new Vector3(position.x, position.y, -position.z), new Vector3(1, 1, 1));
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(-position.x, position.y, position.z), new Vector3(1, 1, 1));
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(new Vector3(-position.x, position.y, -position.z), new Vector3(1, 1, 1));
            ///////////////////////////////////////////////////////////////////////////////
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(position.x, position.y, position.z), new Vector3(position.x, position.y, -position.z));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(-position.x, position.y, position.z), new Vector3(-position.x, position.y, -position.z));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(position.x, position.y, position.z), new Vector3(-position.x, position.y, position.z));
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(new Vector3(position.x, position.y, -position.z), new Vector3(-position.x, position.y, -position.z));
            Gizmos.color = Color.yellow;
            for (float i = -position.x + 1; i < position.x; ++i)
            {
                for (float j = -position.z + 1; j < position.z - 1; ++j)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireCube(new Vector3(i, position.y, j), new Vector3(1, 1, 1));
                }
            }
        }
    }
}
