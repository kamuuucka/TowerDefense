using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class GameDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        GameData myTarget = (GameData)target;
        
        EditorGUILayout.LabelField("Build time", EditorStyles.boldLabel);

        EditorGUILayout.BeginVertical();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Minutes", GUILayout.Width(100));
        EditorGUILayout.LabelField("Seconds", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        myTarget.buildTimeMinutes = EditorGUILayout.IntField(myTarget.buildTimeMinutes, GUILayout.Width(100));
        myTarget.buildTimeSeconds = EditorGUILayout.IntField(myTarget.buildTimeSeconds, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        
       
    }
}
