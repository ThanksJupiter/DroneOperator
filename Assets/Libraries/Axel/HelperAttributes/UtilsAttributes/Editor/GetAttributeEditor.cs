using UnityEngine;
using UnityEditor;

namespace Utils
{
    [CustomPropertyDrawer(typeof(GetAttribute))]
    public class GetAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GetAttribute getAttribute = attribute as GetAttribute;
            MonoBehaviour targetObject = property.serializedObject.targetObject as MonoBehaviour;
      
            using (var scope = new EditorGUILayout.HorizontalScope())
            {
                Component foundComponent;

                EditorGUILayout.PropertyField(property, label, GUILayout.ExpandWidth(true));

                if (property.objectReferenceValue != null)
                    return;

                if (getAttribute.RelativePath.Length == 0)
                {
                    foundComponent = targetObject.GetComponent(fieldInfo.FieldType);

                    if(foundComponent == null && getAttribute.AutoCreateIfMissing)
                    {
                        if (fieldInfo.FieldType == typeof(Collider))
                        {
                            foundComponent = Undo.AddComponent(targetObject.gameObject, typeof(BoxCollider));
                        }
                        else
                        {
                            foundComponent = Undo.AddComponent(targetObject.gameObject, fieldInfo.FieldType);
                        }
                    }
                }
                else if (getAttribute.RelativePath[0] == '*')
                {
                    string path = getAttribute.RelativePath;
                    path = path.Remove(0, 1);
                    Transform targetTransform = FindComponentInAnyChildren(path, targetObject.gameObject);
                    foundComponent = targetTransform?.GetComponent(fieldInfo.FieldType);
                }
                else
                {
                    foundComponent = targetObject.transform.Find(getAttribute.RelativePath)?.GetComponent(fieldInfo.FieldType);
                }

                if (foundComponent == null)
                {
                    if (GUILayout.Button("Create", GUILayout.Width(60)))
                    {
                        if (getAttribute.RelativePath.Length == 0)
                        {
                            if (fieldInfo.FieldType == typeof(Collider))
                            {
                                foundComponent = Undo.AddComponent(targetObject.gameObject, typeof(BoxCollider));
                            }
                            else
                            {
                                foundComponent = Undo.AddComponent(targetObject.gameObject, fieldInfo.FieldType);
                            }
                        }
                        else
                        {
                            Transform targetTransform = FindComponentInChildren(getAttribute, targetObject.gameObject);
                            if (fieldInfo.FieldType == typeof(Collider))
                            {
                                foundComponent = Undo.AddComponent(targetObject.gameObject, typeof(BoxCollider));
                            }
                            else
                            {
                                foundComponent = Undo.AddComponent(targetTransform.gameObject, fieldInfo.FieldType);
                            }
                        }
                    }
                }

                property.objectReferenceValue = foundComponent;
            }

        }

        private bool RelativePathEmpty(string relativePath)
        {
            return relativePath.Length == 0;
        }

        private Transform FindComponentInAnyChildren(string path, GameObject targetObject)
        {
            Transform target = targetObject.transform.Find(path);
            if (target != null)
                return target;

            foreach (Transform child in targetObject.transform)
            {
                target = FindComponentInAnyChildren(path, child.gameObject);
                if (target != null)
                    return target;
            }

            return null;
        }

        private static Transform FindComponentInChildren(GetAttribute getAttribute, GameObject targetObject)
        {
            Transform targetTransform = targetObject.transform.Find(getAttribute.RelativePath);
            if (targetTransform == null)
            {
                targetTransform = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                targetTransform.parent = targetObject.transform;

                string newName = getAttribute.Name;
                if (newName[0] == '*')
                    newName = newName.Remove(0, 1);

                targetTransform.name = newName;
                GameObject.DestroyImmediate(targetTransform.gameObject.GetComponent<MeshFilter>());
                GameObject.DestroyImmediate(targetTransform.gameObject.GetComponent<MeshRenderer>());
                GameObject.DestroyImmediate(targetTransform.gameObject.GetComponent<Collider>());
                Undo.RegisterCreatedObjectUndo(targetTransform.gameObject, "NewEmpty");
            }

            return targetTransform;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0.0f;
        }
    }

}