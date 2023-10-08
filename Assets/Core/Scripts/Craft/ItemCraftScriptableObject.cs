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
        public float timeToCraft;
        public int craftEnergyRequired;
        [Range(1, 10)]
        public int amountToCraft = 1;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemCraftScriptableObject), true)]
    public class ItemCraftScriptableObjectEditor : ItemScriptableObjectEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (serializedObject != null)
            {
                serializedObject.Update();
                ItemCraftScriptableObject scriptableObject = (ItemCraftScriptableObject)target;
                if (scriptableObject != null)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("itemsRequired"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("timeToCraft"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("craftEnergyRequired"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("amountToCraft"));
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}


