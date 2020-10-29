using System;
using Tofunaut.TofuUnity;
using UnityEditor;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    [CustomPropertyDrawer(typeof(RequireTypeAttribute))]
    public class RequireTypePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            RequireTypeAttribute requireTypeAttribute = attribute as RequireTypeAttribute;

            
            var castToRequiredType =
                Convert.ChangeType(property.objectReferenceValue, requireTypeAttribute.requireType);
            
            property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(UnityEngine.Object), true);
            
            
            Debug.Log((castToRequiredType == null).ToString());
        }
        
        public object GetValue(SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);  
            return fi.GetValue(property.serializedObject.targetObject);
        }
    }
}