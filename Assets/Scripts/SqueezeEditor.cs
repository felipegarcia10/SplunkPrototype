using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Squeeze))]
public class SqueezeEditor : Editor
{
    private Squeeze squeeze;
    private float debugValue = 1f;

    private void OnEnable()
    {
        squeeze = (Squeeze)target;
        // Initialize slider from current localScale.y and clamp to valid range
        debugValue = Mathf.Clamp(squeeze.transform.localScale.y, 0.1f, 1f);
    }

    public override void OnInspectorGUI()
    {
        // Draw the default inspector for existing serialized fields
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Squeeze", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        debugValue = EditorGUILayout.Slider("Squeeze Amount", debugValue, 0.1f, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            // Make the change undoable and mark scene dirty
            Undo.RecordObject(squeeze.transform, "Squeeze Debug Change");
            squeeze.UpdateSqueeze(debugValue);
            EditorUtility.SetDirty(squeeze.transform);
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset to 1"))
        {
            Undo.RecordObject(squeeze.transform, "Squeeze Reset");
            squeeze.UpdateSqueeze(1f);
            debugValue = 1f;
            EditorUtility.SetDirty(squeeze.transform);
        }

        EditorGUILayout.EndHorizontal();
    }
}