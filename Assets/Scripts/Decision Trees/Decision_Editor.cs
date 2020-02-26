using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class Decision_Editor : EditorWindow
{
    MonoScript script;
    List<IDecision> tree = new List<IDecision>();
    bool currentlyOnNode = false;
    bool inEditorMode = false;
    bool doOnce = true;
    IDecision editorObject = null;
    string name;
    string[] treeArr;
    [MenuItem("Window/Decision Tree")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Decision_Editor));
    }
    void OnGUI()
    {
        CreateNodes();
        FinishedScreen();
    }
    public void FinishedScreen()
    {
        if (!currentlyOnNode)
        {
            if (inEditorMode)
            {
                EditorMode();
                if (GUILayout.Button("Finished"))
                {
                    inEditorMode = false;
                    editorObject = null;
                }
            }else
            {
                doOnce = true;
                foreach (IDecision t in tree)
                {
                    if (GUILayout.Button(t.ID))
                    {
                        editorObject = t;
                        inEditorMode = true;
                    }
                }
                if (GUILayout.Button("Clear"))
                {
                    tree.Clear();
                }
            }
        }
    }
    public void EditorMode()
    {
        if(editorObject != null && inEditorMode)
        {
            EditorGUILayout.LabelField($"Currently In {editorObject.ID}");
            if (doOnce)
            {
                if (treeArr != null)
                    treeArr = null;
                treeArr = new string[tree.Count+1];
                int count = 0;
                foreach (IDecision t in tree)
                {
                    treeArr[count++] = t.ID;
                }
                treeArr[tree.Count + 1] = "NONE";
                doOnce = false;
            }
            if(treeArr != null)
            {
                editorObject.trueInt = EditorGUILayout.Popup("True: ", editorObject.trueInt, treeArr);
                editorObject.falseInt = EditorGUILayout.Popup("False: ", editorObject.falseInt, treeArr);
                if(editorObject.trueInt == tree.Count + 1)
                    editorObject.SetNode(tree[editorObject.falseInt], null);
                if(editorObject.falseInt == tree.Count + 1)
                    editorObject.SetNode(null, tree[editorObject.trueInt]);
                else
                    editorObject.SetNode(tree[editorObject.falseInt], tree[editorObject.trueInt]);
            }
            editorObject.starting = EditorGUILayout.Toggle("Starting Node: ", editorObject.starting);
            foreach (IDecision t in tree)
            {
                if (t != editorObject && editorObject.starting)
                {
                    DecisionPro.startNode = editorObject;
                    t.starting = false;
                }
            }
        }
    }
    public void CreateNodes()
    {
        if (tree.Count <= 0)
            currentlyOnNode = true;
        if (currentlyOnNode)
        {
            name = EditorGUILayout.TextField("Name:", name);
            script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
            if (script != null && script.GetClass().IsSubclassOf(typeof(IDecision)))
            {
                IDecision tmp = (IDecision)ScriptableObject.CreateInstance(script.GetClass());
                if (GUILayout.Button("Create Node"))
                {
                    currentlyOnNode = true;
                    tmp.ID = name;
                    tree.Add(tmp);
                    script = null;
                    name = "";
                }
                if (GUILayout.Button("Finished"))
                {
                    currentlyOnNode = false;
                    tmp.ID = name;
                    tree.Add(tmp);
                    script = null;
                    name = "";
                }
            }
            else
            {
                if (GUILayout.Button("Finished"))
                {
                    currentlyOnNode = false;
                    name = "";
                    script = null;
                }
            }
        }
    }
}
