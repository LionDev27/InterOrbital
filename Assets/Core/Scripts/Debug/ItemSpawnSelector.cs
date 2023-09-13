using System.Collections.Generic;
using InterOrbital.Item;
using InterOrbital.Player;
using TMPro;
using UnityEngine;

public class ItemSpawnSelector : MonoBehaviour
{
    private TMP_Dropdown _dropdown;
    private Dictionary<string, ItemScriptableObject> _options = new();

    private void Awake()
    {
        if (Application.isEditor)
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            SetupDropdown();
        }
    }

    private void SetupDropdown()
    {
        var resources = Resources.LoadAll<ItemScriptableObject>("");
        foreach (var scriptableObject in resources)
        {
            var objectName = scriptableObject.name;
            _dropdown.options.Add(new TMP_Dropdown.OptionData(objectName));
            _options.Add(objectName, scriptableObject);
        }
    }

    public void SpawnSelected()
    {
        if (_options.TryGetValue(_dropdown.options[_dropdown.value].text, out var itemSO))
        {
            var player = PlayerComponents.Instance;
            player.Inventory.DropItem(player.PlayerAttack.attackPoint.position, player.transform.position, -1, itemSO);
        }
    }
}
