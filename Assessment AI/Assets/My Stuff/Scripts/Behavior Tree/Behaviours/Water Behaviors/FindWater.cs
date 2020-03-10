using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behaviors/FindWater")]
public class FindWater : Behavior
{
    public override bool Execute(GameObject p, List<string> tags)
    {
        Animal ani = p.GetComponent<Animal>();
        if (ani != null)
        {
            if (ani.GetWater() <= ani.SeekWater() && ani.GetWater() <= ani.GetFood())
            {
                if (!ani.reachedPoint || !ani.PathFound())
                {
                    ani.reachedPoint = true;
                    ani.target = ani.GetWaterTarget();
                    ani.FindPath(ani.target,tags);
                }
                if (ani.target != null)
                {
                    if (Vector3.Distance(p.transform.position, ani.target.transform.position) > 1f)
                    {
                        ani.MoveTowards();
                    }
                    else
                    {
                        ani.water = ani.maxWater;
                        ani.reachedPoint = false;
                        ani.ClearPath();
                    }
                }
                return true;
            }
        }
        return false;
    }
}
