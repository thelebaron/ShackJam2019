using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace Game
{
    [CustomEditor(typeof(Door))]
    public class DoorInspector : Editor
    {
        private void OnSceneGUI()
        {
            //var fracture = target as FractureAuthoring;

            /*
            Handles.color = Color.magenta;
            Handles.color = Color.red;
            Handles.CircleHandleCap(0,fracture.transform.position + new Vector3(5, 0, 0),fracture.transform.rotation * Quaternion.LookRotation(new Vector3(1, 0, 0)),1,EventType.Repaint);


            //
            //text
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.green;

            Handles.BeginGUI();
            Vector3 pos = fracture.transform.position;
            Vector2 pos2D = HandleUtility.WorldToGUIPoint(pos);
            GUI.Label(new Rect(pos2D.x, pos2D.y, 100, 100), pos.ToString(), style);
            Handles.EndGUI();
            //

            EditorGUI.BeginChangeCheck();
            
            for (int i = 0; i < fracture.m_Points.Count; i++)
            {
                Handles.color = Color.magenta;
                fracture.m_HandlePoints[i] = Handles.PositionHandle(fracture.m_HandlePoints[i], Quaternion.identity);
                fracture.Update();
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Look Target");
                //floor.lookTarget = lookTarget;
                
            }*/
        }

        public override void OnInspectorGUI()
        {
            var door = target as Door;
            
            if (GUILayout.Button("Set closed state"))
            {
                if (door != null) 
                    door.SetClosedState(); //Refresh in editor view
            }
            if (GUILayout.Button("Set open state"))
            {
                if (door != null) 
                    door.SetOpenState(); //Refresh in editor view
            }
            
            DrawDefaultInspector();

        }
        
    }
}