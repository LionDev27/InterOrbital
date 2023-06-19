using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemFuelScriptableObject", order = 6)]
    public class ItemFuelScriptableObject : ItemScriptableObject
    {
        public int energyProvided;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemFuelScriptableObject), true)]
    public class ItemFuelScriptableObjectEditor : ItemScriptableObjectEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (serializedObject != null)
            {
                serializedObject.Update();
                ItemFuelScriptableObject scriptableObject = (ItemFuelScriptableObject) target;
                if (scriptableObject != null)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("energyProvided"));
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}

