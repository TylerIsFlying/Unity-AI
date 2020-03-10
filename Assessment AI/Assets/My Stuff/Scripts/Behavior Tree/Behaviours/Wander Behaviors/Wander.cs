using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behaviors/Wander")]
public class Wander : Behavior
{
    // Getting a new location
    public void NewLoc(Animal p, List<string> tags)
    {
       p.animalPoint.transform.position = (Random.insideUnitSphere * 5) + p.transform.position;
        p.animalPoint.transform.position = new Vector3(p.animalPoint.transform.position.x, p.transform.position.y, p.animalPoint.transform.position.z);
       p.FindPath(p.animalPoint, tags);
    }
    public override bool Execute(GameObject p, List<string> tags)
    {
        Animal ani = p.GetComponent<Animal>();
        if(ani != null && ani.GetFood() > ani.SeekFood() && ani.GetWater() > ani.SeekWater())
        {
            // checking distance and if it less than 1f then it switches to a new path
            if (Vector3.Distance(p.transform.position, ani.animalPoint.transform.position) <= 1f || !ani.PathFound() || ani.MoveTowards())
            {
                NewLoc(ani, tags);
            }
            Debug.DrawLine(p.transform.position, ani.animalPoint.transform.position);
            return true;
        }
        return false;
    }
}
