using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor
{
    private static bool showEntityActions = false;
    private ReorderableList _list;
    private SerializedProperty _entityActionList;

    private void OnEnable()
    {
        _entityActionList = serializedObject.FindProperty("entityActions");
        _list = new ReorderableList(serializedObject, _entityActionList, true, true, true, true)
        {
            drawHeaderCallback = DrawListHeader,
            drawElementCallback = DrawListElement
        };
    }

    private void DrawListHeader(Rect rect)
    {
        GUI.Label(rect, "Entity Actions");
    }

    private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var item = _entityActionList.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, item);
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        SerializedObject so = serializedObject;

        SerializedProperty entityName = so.FindProperty("entityName");
        SerializedProperty entityType = so.FindProperty("entityType");
        SerializedProperty entityActions = so.FindProperty("entityActions");

        EditorGUILayout.PropertyField(entityName);
        EditorGUILayout.PropertyField(entityType);
        // _list.
        _list.DoLayoutList();

        showEntityActions = EditorGUILayout.Foldout(showEntityActions, "Entity Actions Settings");

        if (showEntityActions)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < entityActions.arraySize; i++)
            {
                var element = entityActions.GetArrayElementAtIndex(i).objectReferenceValue as EntityAction;
                EditorGUILayout.LabelField($"Element {i}");
                element = EditorGUILayout.ObjectField(element, typeof(EntityAction), false) as EntityAction;
                if (element == null) continue;
                var fields = element.GetType().GetFields();
                EditorGUI.indentLevel++;
                for (int j = 0; j < fields.Length; j++)
                {
                    var field = fields[j];
                    // handle bool
                    if (field.FieldType == typeof(System.Boolean))
                    {
                        bool b = (bool)field.GetValue(element);
                        bool bf = EditorGUILayout.Toggle(field.Name, b);
                        field.SetValue(element, bf);
                    }

                    // handle float
                    if (field.FieldType == typeof(System.Single))
                    {
                        float f = (float)field.GetValue(element);
                        float ff = EditorGUILayout.FloatField(field.Name, f);
                        field.SetValue(element, ff);
                    }

                    // handle Vector3
                    if (field.FieldType == typeof(UnityEngine.Vector3))
                    {
                        Vector3 v = (Vector3)field.GetValue(element);
                        Vector3 vf = EditorGUILayout.Vector3Field(field.Name, v);
                        field.SetValue(element, vf);
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        so.ApplyModifiedProperties();
    }
}