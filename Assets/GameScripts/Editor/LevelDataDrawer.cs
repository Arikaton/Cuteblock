using System.Collections.Generic;
using GameScripts.Misc;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Game
{
    [CustomPropertyDrawer(typeof(LevelData))]
    public class LevelDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            var listProperty = property.FindPropertyRelative(nameof(LevelData.cellsWithGems));
            var list = SerializedFieldHelper.GetTargetObjectOfProperty(listProperty) as List<Vector2Int>;
            
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(6));
            for (int x = 0; x < 9; x++)
            {
                EditorGUILayout.BeginVertical();
                for (int y = 8; y >= 0; y--)
                {
                    bool pointExists = list.Contains(new Vector2Int(x, y));
                    var newToggle = GUILayout.Toggle(pointExists, $"{x},{y}", new GUIStyle("Button"), GUILayout.Height(30), GUILayout.Width(30));
                    
                    if (pointExists && newToggle == false)
                        list.Remove(new Vector2Int(x, y));
                    if (!pointExists && newToggle)
                        list.Add(new Vector2Int(x, y));
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(property.serializedObject.targetObject, "Level data change");
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            
            GUI.enabled = false;
            EditorGUILayout.PropertyField(listProperty);
            GUI.enabled = true;
        }
    }
}