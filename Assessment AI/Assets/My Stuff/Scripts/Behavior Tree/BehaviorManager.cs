using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour
{
    public BehaviorScript behaviorStarter; // behaviour that starts
    public BehaviorSettings settings; // used to get the settings of the object you want to create and this class manages those objects2
    private void Start()
    {
        if (settings != null)
        {
            settings.Start();
            if (settings.objects != null)
            {
                foreach (GameObject o in settings.objects)
                {
                    if (behaviorStarter != null) o.GetComponent<Behaviors>().behaviorStarter = behaviorStarter;
                }
            }
        }
    }
    //updating the objects and there behaviours
    public void Update()
    {
        if(settings != null)
        {
            if (settings.objects != null)
            {
                foreach (GameObject o in settings.objects)
                {
                    Behaviors tmp = o.GetComponent<Behaviors>();
                    if (tmp != null)
                    {
                        if(settings.tags.Count <= 0)
                        {
                            if (tmp.currentBehavior != null) tmp.currentBehavior.GetChildren(out tmp.currentBehavior, o);
                            else tmp.currentBehavior = tmp.behaviorStarter;
                        }
                        else
                        {
                            if (tmp.currentBehavior != null) tmp.currentBehavior.GetChildren(out tmp.currentBehavior, o, settings.tags);
                            else tmp.currentBehavior = tmp.behaviorStarter;
                        }
                    }
                }
            }
        }
    }
}
