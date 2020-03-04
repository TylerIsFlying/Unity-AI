using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : IBehavior
{
    // need to work on wander
    private GameObject wanderPoint;
    private bool doOnce = false;
    // Getting a new location
    public void NewLoc(Animal p, List<string> tags)
    {
       wanderPoint.transform.position = (Random.insideUnitSphere * 5) + p.transform.position;
        wanderPoint.transform.position = new Vector3(wanderPoint.transform.position.x, p.transform.position.y, wanderPoint.transform.position.z);
       p.FindPath(wanderPoint,tags);
    }
    public override bool Execute(GameObject p, List<string> tags)
    {
        if (!Application.isPlaying) doOnce = false;
        Animal ani = p.GetComponent<Animal>();
        if(ani != null)
        {
            if (!doOnce)
            {
                if(wanderPoint == null) wanderPoint = new GameObject("Wander");
                doOnce = true;
                NewLoc(ani,tags);
        }
            if (Vector3.Distance(p.transform.position, wanderPoint.transform.position) <= 1f || !ani.PathFound())
                NewLoc(ani,tags);
            ani.MoveTowards();
            Debug.DrawLine(p.transform.position, wanderPoint.transform.position);
        }
        return true;
    }
}
