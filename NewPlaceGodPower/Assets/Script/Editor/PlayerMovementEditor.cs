using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    /*
    public override void OnInspectorGUI()
    {
        GUIStyle centeredStyle = GUI.skin.GetStyle("HeaderLabel");
        centeredStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Your Header Text", centeredStyle);

        DrawDefaultInspector();
    }
    */
}
