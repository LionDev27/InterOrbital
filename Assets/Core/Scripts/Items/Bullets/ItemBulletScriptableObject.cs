using UnityEngine;
using UnityEditor;

namespace InterOrbital.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemBulletScriptableObject", order = 2)]
    public class ItemBulletScriptableObject : ItemCraftScriptableObject
    {
        public GameObject bulletPrefab;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemBulletScriptableObject), true)]
    public class ItemBulletScriptableObjectEditor : ItemCraftScriptableObjectEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (serializedObject != null)
            {
                serializedObject.Update();
                ItemBulletScriptableObject scriptableObject = (ItemBulletScriptableObject)target;
                if (scriptableObject != null)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletPrefab"));
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}
