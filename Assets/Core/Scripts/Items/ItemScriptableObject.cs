using UnityEngine;
using InterOrbital.Utils;
using UnityEditor;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite itemSprite;
        public int id;
        public string itemName;
        public string itemDescription;
        public TypeCraft type;
        public bool isStackable;
        public int maxAmount;
        public GameObject buildPrefab;
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ItemScriptableObject),true)]
    public class ItemScriptableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (serializedObject != null)
            {
                serializedObject.Update();
                ItemScriptableObject scriptableObject = (ItemScriptableObject)target;
                if (scriptableObject != null)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemSprite"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isStackable"));


                    if (scriptableObject.isStackable)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmount"));
                    }

                    if (scriptableObject.type == TypeCraft.Build)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("buildPrefab"));
                    }
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}
