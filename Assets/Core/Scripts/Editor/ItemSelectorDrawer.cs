using System;
using System.Linq;
using InterOrbital.Item;
using UnityEditor;
using UnityEngine;

namespace InterOrbital.EditorTools
{
    [CustomPropertyDrawer(typeof(ItemSelector))]
    public class ItemSelectorDrawer : PropertyDrawer
    {
        private static string[] _itemChoices;
        private static string[] ItemChoices
        {
            get
            {
                if (_itemChoices == null)
                {
                    var items = FindItems();
                    _itemChoices = items;
                }

                return _itemChoices;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyString = property.FindPropertyRelative("itemName").stringValue;
            var choiceIndex = propertyString == null ? 0 : Array.IndexOf(ItemChoices, propertyString);
            if (choiceIndex < 0)
                choiceIndex = 0;

            EditorGUI.BeginProperty(position, label, property);

            choiceIndex = EditorGUI.Popup(position, choiceIndex, ItemChoices);
            property.FindPropertyRelative("itemName").stringValue = ItemChoices[choiceIndex];

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        private static string[] FindItems()
        {
            var allItems = Resources.LoadAll("DropItems");
            string[] itemNames = allItems.Select(x => x.name).ToArray();
            return itemNames;
        }
    }
}