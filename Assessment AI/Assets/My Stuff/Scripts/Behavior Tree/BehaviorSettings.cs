using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Behavior Tree", menuName = "Behavior Tree/Settings")]
public class BehaviorSettings : ScriptableObject
{
    // settings for the prefabs and like targets tag when I use it will be planning for future use
    [Header("Prefab Settings")]
    public GameObject prefab = null;
    public Vector3 startPosition = Vector3.zero;
    public int spawnedPrefabs = 0;
    [Header("Targets To Avoid")]
    public List<string> tags;
    [HideInInspector]
    public List<GameObject> objects = new List<GameObject>();
    // start creates the gameobject
    public void Start()
    {
        objects.Clear();
        for (int i = 0; i < spawnedPrefabs; i++)
        {
            GameObject o = Instantiate(prefab);
            o.transform.position = startPosition;
            o.AddComponent<Behaviors>();
            objects.Add(o);
        }
    }
}
