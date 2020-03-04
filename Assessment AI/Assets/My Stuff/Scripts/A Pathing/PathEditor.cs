using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathEditor : EditorWindow
{
    [MenuItem("Window/Path Editor")]
    static void Init()
    {
        PathEditor editor = (PathEditor)EditorWindow.GetWindow(typeof(PathEditor));
    }
    void OnGUI()
    {
        if(GUILayout.Button("Create Node"))
        {

        }
    }
}
