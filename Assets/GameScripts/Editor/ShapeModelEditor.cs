using System;
using System.Collections.Generic;
using GameScripts.Misc;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Game
{
    [CustomPropertyDrawer(typeof(ShapeData))]
    public class ShapeModelEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var uidProp = property.FindPropertyRelative("uid");
            var rectProp = property.FindPropertyRelative("rect");
            var pointsProp = property.FindPropertyRelative("points");
            var rotationProp = property.FindPropertyRelative("rotation");
            
            var xProp = rectProp.FindPropertyRelative(nameof(Vector2Int.x));
            var yProp = rectProp.FindPropertyRelative(nameof(Vector2Int.y));
            
            EditorGUILayout.PropertyField(uidProp);
            var rectValue = EditorGUILayout.Vector2IntField("Rect", new Vector2Int(xProp.intValue, yProp.intValue));
            xProp.intValue = Math.Clamp(rectValue.x, 1, 9);
            yProp.intValue = Math.Clamp(rectValue.y, 1, 9);

            var list = SerializedFieldHelper.GetTargetObjectOfProperty(pointsProp) as List<Vector2Int>;
            RemoveEntriesOutOfRectBounds(list, xProp.intValue, yProp.intValue);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(6));
            for (int x = 0; x < xProp.intValue; x++)
            {
                EditorGUILayout.BeginVertical();
                for (int y = yProp.intValue - 1; y >= 0; y--)
                {
                    bool pointExists = list.Contains(new Vector2Int(x, y));
                    var newToggle = GUILayout.Toggle(pointExists, $"{x},{y}", new GUIStyle("Button"), GUILayout.Height(30), GUILayout.Width(30));
                    if (pointExists && newToggle == false)
                        list.Remove(new Vector2Int(x, y));
                    if (!pointExists && newToggle)
                        list.Add(new Vector2Int(x, y));
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(pointsProp);
            EditorGUILayout.PropertyField(rotationProp);
            GUI.enabled = true;
        }

        private void RemoveEntriesOutOfRectBounds(List<Vector2Int> list, int x, int y)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].x >= x || list[i].y >= y)
                    list.RemoveAt(i);
            }
        }
    }
}