using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Utils;
using UnityEditor;

namespace InterOrbital.Item
{
    
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemCraftScriptableObject", order = 1)]
    public class ItemCraftScriptableObject : ItemScriptableObject
    {
        public List<ItemRequired> itemsRequired;
    }

    [CustomEditor(typeof(ItemCraftScriptableObject))]
    public class ItemCraftScriptableObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (serializedObject != null)
            {
                serializedObject.Update();

                ItemScriptableObject scriptableObject = (ItemScriptableObject)target;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemSprite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("id"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemDescription"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isStackable"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmount"));


                if (scriptableObject.type == TypeCraft.Build)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("buildPrefab"));
                }

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}


