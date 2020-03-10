using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selector : MonoBehaviour
{
    public Camera myCamera;
    public string targetTag;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI idText;
    public int senstivity = 4;
    public int minFov = 15;
    public int maxFov = 90;
    GameObject[] targets;
    int currentTarget = 0;
    int lastCurrentTarget = 0;
    bool isPressed = false;
    float fov = 0;
    Vector3 offset;
    Animal[] anis;
    void Start()
    {
        fov = myCamera.fieldOfView;
    }
    void Update()
    {
        if(targets == null)
        {
            targets = GameObject.FindGameObjectsWithTag(targetTag);
            if (targets.Length > 0) offset = myCamera.transform.position - targets[currentTarget].transform.position;
            anis = new Animal[targets.Length];
            for(int i = 0; i < targets.Length; i++)
            {
                if(targets[i].GetComponent<Animal>() != null) anis[i] = targets[i].GetComponent<Animal>();
            }
        }
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
        }
        if(anis != null && anis.Length > 0 && anis[currentTarget] != null)
        {
            if(healthText != null)healthText.text = $"Health: {anis[currentTarget].health}";
            if(waterText != null)waterText.text = $"Water: {anis[currentTarget].water}";
            if(foodText != null)foodText.text = $"Food: {anis[currentTarget].food}";
            if (idText != null) idText.text = $"ID: {anis[currentTarget].id}";
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
