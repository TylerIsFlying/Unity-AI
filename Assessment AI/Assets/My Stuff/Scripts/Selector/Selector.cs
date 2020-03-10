using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selector : MonoBehaviour
{
    public Camera myCamera; // this is my camera
    public string targetTag; // this will be wqhat the target tag is
    public GameObject prefabSphere; // used to get the prefab for the object that will represent the nodes
    public TextMeshProUGUI healthText; // text to display health
    public TextMeshProUGUI waterText; // text to display water
    public TextMeshProUGUI foodText; // text to display food
    public TextMeshProUGUI idText; // text to display id
    public TextMeshProUGUI behaviorText; // text to display behavior
    public int senstivity = 4; // used for the scroll wheel
    public int minFov = 15; // miniumn field of view
    public int maxFov = 90; // max field of view
    GameObject[] targets; // array of targets
    Behaviors[] behaviors; // array of behaviors
    int currentTarget = 0; // current target
    int lastCurrentTarget = 0; // target before the currentone
    bool isPressed = false; // used for a trigger for the button
    float fov = 0; // used to have the fov
    Vector3 offset; // offset for the camera
    Animal[] anis; // list of all the animals
    List<AniSettings> aniSettings = new List<AniSettings>();
    void Start()
    {
        fov = myCamera.fieldOfView;
    }
    void Update()
    {
        if(targets == null)
        {
            // setting up the targets and other stuff
            targets = GameObject.FindGameObjectsWithTag(targetTag);
            if (targets.Length > 0) offset = myCamera.transform.position - targets[currentTarget].transform.position;
            anis = new Animal[targets.Length];
            behaviors = new Behaviors[targets.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                if(targets[i].GetComponent<Animal>() != null) anis[i] = targets[i].GetComponent<Animal>();
                if (targets[i].GetComponent<Behaviors>() != null) behaviors[i] = targets[i].GetComponent<Behaviors>();
            }
        }
        // used for the keys to go to other chickens and to zoom in and out
        if(targets != null && targets.Length > 0)
        {
            lastCurrentTarget = currentTarget;
            fov += Input.GetAxis("Mouse ScrollWheel") * senstivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isPressed)
            {
                isPressed = true;
                currentTarget--;
                if (currentTarget < 0) currentTarget = targets.Length - 1;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) && isPressed)
            {
                isPressed = false;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isPressed)
            {
                isPressed = true;
                currentTarget++;
                if (currentTarget >= targets.Length) currentTarget = 0;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) && isPressed)
            {
                isPressed = false;
            }
            if(prefabSphere != null) ShowNodes();
        }
        // just checking the distance for the target
        for (int i = 0; i < aniSettings.Count; i++)
        {
            if (aniSettings[i].o != null && Vector3.Distance(anis[currentTarget].transform.position, aniSettings[i].o.transform.position) <= 0.5f)
            {
                aniSettings[i].render.material.SetColor("_Color", Color.green);
            }
            else aniSettings[i].render.material.SetColor("_Color", aniSettings[i].color);
        }
        // showing the text and setting the values for it
        if (anis != null && anis.Length > 0 && anis[currentTarget] != null)
        {
            if(healthText != null)healthText.text = $"Health: {anis[currentTarget].health}";
            if(waterText != null)waterText.text = $"Water: {anis[currentTarget].water}";
            if(foodText != null)foodText.text = $"Food: {anis[currentTarget].food}";
            if (idText != null) idText.text = $"ID: {anis[currentTarget].id}";
            if (behaviorText != null && behaviors[currentTarget].currentBehavior.id != behaviors[currentTarget].behaviorStarter.id) behaviorText.text = $"Behavior: {behaviors[currentTarget].currentBehavior.id}";
        }
    }
    void ShowNodes()
    {
        if(lastCurrentTarget != currentTarget || aniSettings.Count <= 0)
        {
            for (int i = 0; i < aniSettings.Count; i++)
            {
                Destroy(aniSettings[i].o);
            }
            aniSettings.Clear();
            for (int i = 0; i < anis[currentTarget].path.Count; i++)
            {
                GameObject tmp = Instantiate(prefabSphere);
                tmp.transform.position = anis[currentTarget].path[i].position;
                Color color = Random.ColorHSV();
                aniSettings.Add(new AniSettings(tmp, tmp.GetComponent<Renderer>(), color));
                aniSettings[i].render.material.SetColor("_Color", aniSettings[i].color);
            }
        }
        else
        {
            for (int i = 0; i < anis[currentTarget].path.Count; i++)
            {
                if (i >= aniSettings.Count)
                {
                    GameObject tmp = Instantiate(prefabSphere);
                    tmp.transform.position = anis[currentTarget].path[i].position;
                    Color color = Random.ColorHSV();
                    aniSettings.Add(new AniSettings(tmp, tmp.GetComponent<Renderer>(), color));
                    aniSettings[i].render.material.SetColor("_Color", aniSettings[i].color);
                }
                else if (anis[currentTarget].path.Count < aniSettings.Count - 1)
                {
                    for (int j = anis[currentTarget].path.Count; j < aniSettings.Count - 1; j++)
                    {
                        Destroy(aniSettings[j].o);
                        aniSettings.Remove(aniSettings[j]);
                    }
                }
                if(aniSettings[i].o != null)
                {
                    aniSettings[i].o.transform.position = anis[currentTarget].path[i].position;
                }
            }
        }
    }
    void LateUpdate()
    {
        if (targets != null && targets.Length > 0)
        {
            myCamera.transform.position = (targets[currentTarget].transform.position + offset);
            myCamera.fieldOfView = fov;
        }
    }
}
