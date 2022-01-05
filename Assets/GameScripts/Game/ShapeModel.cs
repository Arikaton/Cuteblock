using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace GameScripts.Game
{ 
    [System.Serializable]
    public struct ShapeModel
    {
        public int uid;
        public Vector2IntSerializable rect;
        [SerializeField] private List<Vector2IntSerializable> points;
        [SerializeField] private Rotation rotation;
    }
    
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ShapeModel))]
    public class ShapeModelEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
    
    #endif
}