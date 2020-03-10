using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Behaviors/FindFood")]
public class FindFood : Behavior
{
    public override bool Execute(GameObject p, List<string> tags)
    {
        Animal ani = p.GetComponent<Animal>();
        if (ani != null)
        {
            if(ani.GetFood() <= ani.SeekFood() && ani.GetFood() <= ani.GetWater())
            {
                if (!ani.reachedPoint || !ani.PathFound())
                {
                    ani.reachedPoint = true;
                    ani.target = ani.GetFoodTarget();
                    ani.FindPath(ani.target, tags);

                }
                if (ani.target != null)
                {
                    if (Vector3.Distance(p.transform.position, ani.target.transform.position) > 1f)
                    {
                        ani.MoveTowards();
                    }
                    else
                    {
                        ani.food = ani.maxFood;
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
