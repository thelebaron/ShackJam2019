using UnityEngine;
using UnityEditor;

#if (UNITY_EDITOR) 
[CanEditMultipleObjects]
[CustomEditor(typeof(Transform), true)]
public class NewTransformInspector : Editor
{

    /// <summary>
    /// Draw the inspector widget.
    /// </summary>
    public override void OnInspectorGUI()
    {
        Transform t = (Transform)target;

        float oldLabelWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = 15;

        EditorGUILayout.BeginHorizontal();
        bool resetPos = GUILayout.Button("P", GUILayout.Width(20f));
        Vector3 position = EditorGUILayout.Vector3Field("", t.localPosition);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bool resetRot = GUILayout.Button("R", GUILayout.Width(20f));
        Vector3 eulerAngles = EditorGUILayout.Vector3Field("", t.localEulerAngles);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        bool resetScale = GUILayout.Button("S", GUILayout.Width(20f));
        Vector3 scale = EditorGUILayout.Vector3Field("", t.localScale);
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = oldLabelWidth;

        if (resetPos) position = Vector3.zero;
        if (resetRot) eulerAngles = Vector3.zero;
        if (resetScale) scale = Vector3.one;

        if (GUI.changed)
        {
            Undo.RecordObject(t, "Transform Change");
            
            t.localEulerAngles = FixIfNaN(eulerAngles);
            t.localScale = FixIfNaN(scale);


            t.localPosition = FixIfNaN(position);
        }
    }

    private Vector3 FixIfNaN(Vector3 v)
    {
        if (float.IsNaN(v.x))
        {
            v.x = 0;
        }
        if (float.IsNaN(v.y))
        {
            v.y = 0;
        }
        if (float.IsNaN(v.z))
        {
            v.z = 0;
        }
        return v;
    }

    public void Update()
    {

        SceneView.duringSceneGui += OnScene;
        /*
        if (SceneView.onSceneGUIDelegate == null)
        {
            //Any other initialization code you would
            //normally place in Init() goes here instead.
        }*/

    }

    private static void OnScene(SceneView sceneview)
    {
        EditorGUI.BeginChangeCheck();


        EditorGUI.EndChangeCheck();
    }

}

#endif