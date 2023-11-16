using InterOrbital.Item;
using InterOrbital.Recollectables;
using UnityEditor;
using UnityEngine;

namespace InterOrbital.EditorTools
{
    [CustomPropertyDrawer(typeof(RecollectableConfig))]
    public class RecollectableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var itemsProperty = property.FindPropertyRelative("dropItems");
            var ratesProperty = property.FindPropertyRelative("dropRates");
            var itemSelectors = property.FindPropertyRelative("itemSelectors");
            var tierProperty = property.FindPropertyRelative("tier");

            EditorGUI.BeginProperty(position, label, property);

            if (GUILayout.Button("Add Drop Item"))
            {
                itemsProperty.InsertArrayElementAtIndex(0);
                ratesProperty.InsertArrayElementAtIndex(0);
                itemSelectors.InsertArrayElementAtIndex(0);
                SetArrayPropertyValues(ratesProperty, 0);
            }

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField("Drop Items");
            EditorGUILayout.LabelField("Drop Rate");
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();

            int size = itemsProperty.arraySize;

            for (int i = 0; i < size; i++)
            {
                SerializedProperty itemSelector = itemSelectors.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(itemSelector);
                string selectedItemName = itemSelector.FindPropertyRelative("itemName").stringValue;
                var item = Resources.Load("DropItems/" + selectedItemName);
                itemsProperty.GetArrayElementAtIndex(i).objectReferenceValue = item;
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            for (int i = 0; i < size; i++)
            {
                var rate = ratesProperty.GetArrayElementAtIndex(i);
                int newValue = EditorGUILayout.IntField(rate.intValue);
                if (newValue != rate.intValue)
                {
                    rate.intValue = newValue;

                    int sum = CalculateArrayPropertySum(ratesProperty);
                    if (sum > 100)
                    {
                        int equalValue = sum / ratesProperty.arraySize;
                        while (equalValue * ratesProperty.arraySize > 100)
                        {
                            equalValue--;
                        }
                        SetArrayPropertyValues(ratesProperty, equalValue);
                    }
                }

                if (rate.intValue < 0)
                {
                    rate.intValue = 0;
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            for (int i = 0; i < size; i++)
            {
                if (GUILayout.Button("Delete"))
                {
                    itemsProperty.DeleteArrayElementAtIndex(i);
                    ratesProperty.DeleteArrayElementAtIndex(i);
                    itemSelectors.DeleteArrayElementAtIndex(i);
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.HelpBox("Si el conjunto de DropRate supera 100, se igualará el valor de todos.", MessageType.Info);

            EditorGUILayout.PropertyField(tierProperty);

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        private void SetArrayPropertyValues(SerializedProperty array, int newValue)
        {
            foreach (SerializedProperty r in array)
            {
                r.intValue = newValue;
            }
        }
        
        private int CalculateArrayPropertySum(SerializedProperty array)
        {
            int sum = 0;

            foreach (SerializedProperty rate in array)
            {
                sum += rate.intValue;
            }

            return sum;
        }
    }
}