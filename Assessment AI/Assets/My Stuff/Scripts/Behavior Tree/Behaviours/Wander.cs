using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : IBehavior
{
    // need to work on wander
    private GameObject wanderPoint;
    private bool doOnce = false;
    // Getting a new location
    public void NewLoc(Animal p)
    {
        wanderPoint.transform.position = new Vector3(Random.Range(p.transform.position.x - 5, p.transform.position.x + 5),
            p.transform.position.y, 
            Random.Range(p.transform.position.x - 5, p.transform.position.x + 5));
       p.FindPath(wanderPoint);
    }
    public override bool Execute(GameObject p)
    {
        if (!Application.isPlaying) doOnce = false;
        Animal ani = p.GetComponent<Animal>();
        if(ani != null)
        {
            if (!doOnce)
            {
                wanderPoint = new GameObject("Wander");
                doOnce = true;
                NewLoc(ani);
        }
            if (Vector3.Distance(p.transform.position, wanderPoint.transform.position) <= 0.5f)
                NewLoc(ani);
            ani.MoveTowards();
            
            Debug.DrawLine(p.transform.position, wanderPoint.transform.position);
        }
        return true;
    }
}
