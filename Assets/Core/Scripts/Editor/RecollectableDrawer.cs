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

            EditorGUI.BeginProperty(position, label, property);

            //Añadir y eliminar items.

            if (GUILayout.Button("Add Drop Item"))
            {
                itemsProperty.InsertArrayElementAtIndex(0);
                ratesProperty.InsertArrayElementAtIndex(0);
                //Poner todos los rates a 0.
                //Hacer selector de items.
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();

            int size = itemsProperty.arraySize;

            for (int i = 0; i < size; i++)
            {
                var item = itemsProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(item);
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

                    int sum = CalculateRateSum(ratesProperty);
                    while (sum > 100)
                    {
                        int equalValue = sum / ratesProperty.arraySize;
                        foreach (SerializedProperty r in ratesProperty)
                        {
                            r.intValue = equalValue;
                        }
                        
                        sum = CalculateRateSum(ratesProperty);
                    }
                }

                if (rate.intValue < 0)
                {
                    rate.intValue = 0;
                }
            }


            // if (sum > 100)
            // {
            //     int diffPerElement = diff / ratesProperty.arraySize;
            //     for (int i = 0; i < ratesProperty.arraySize; i++)
            //     {
            //         var rate = ratesProperty.GetArrayElementAtIndex(i);
            //         rate.intValue -= diffPerElement;
            //     }
            // }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            for (int i = 0; i < size; i++)
            {
                if (GUILayout.Button("Delete"))
                {
                    itemsProperty.DeleteArrayElementAtIndex(i);
                    ratesProperty.DeleteArrayElementAtIndex(i);
                }
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        private int CalculateRateSum(SerializedProperty ratesArray)
        {
            int sum = 0;

            foreach (SerializedProperty rate in ratesArray)
            {
                sum += rate.intValue;
            }

            return sum;
        }
    }
}