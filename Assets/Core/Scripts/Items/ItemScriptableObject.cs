using UnityEngine;
using InterOrbital.Utils;
using UnityEditor;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 0)]
    public class ItemScriptableObject : ScriptableObject
    {
        public Sprite itemSprite;
        public int id;
        public string itemName;
        [TextArea(1, 2)]
        public string itemDescription;
        public ItemType type;
        public ConsumableValues consumableValues;
        public UpgradeType upgradeType;
        public bool isStackable;
        public int maxAmount;
        public Sprite buildHighlightSprite;
        public GameObject buildPrefab;
        public AnimatorOverrideController animator;
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
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));


                    if (scriptableObject.isStackable)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmount"));
                    }

                    if (scriptableObject.type == ItemType.Consumable)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("consumableValues"));
                    }

                    if (scriptableObject.type == ItemType.Upgrade)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("upgradeType"));
                    }

                    if (scriptableObject.type == ItemType.Build)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("buildHighlightSprite"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("buildPrefab"));
                    }
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}
