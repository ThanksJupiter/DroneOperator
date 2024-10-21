using UnityEngine;
using UnityEditor;

namespace Utils
{
    [CustomPropertyDrawer(typeof(LoadAssetAttribute))]
    public class LoadAssetPropertyDrawerEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
            if (property.objectReferenceValue == null)
            {
                LoadAssetAttribute loadResourceAttribute = attribute as LoadAssetAttribute;
                property.objectReferenceValue = Resources.Load(loadResourceAttribute?.assetName, fieldInfo.FieldType);
            }
        }
    }
}