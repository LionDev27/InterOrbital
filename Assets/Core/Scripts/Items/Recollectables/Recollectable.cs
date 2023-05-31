using System;
using System.Collections.Generic;
using InterOrbital.Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace InterOrbital.Recollectables
{
    public class Recollectable : MonoBehaviour
    {
        [SerializeField] private RecollectableScriptableObject _scriptableObject;
        [Tooltip("How many times can the player recollect from this recollectable.")]
        [SerializeField] private int _health;
        [SerializeField] private GameObject _dropItemPrefab;
        [SerializeField] private float _dropForce = 3f;

        private int _currentHealth;
        private List<ItemScriptableObject> _dropItems => _scriptableObject.recollectableConfig.dropItems;
        private List<int> _dropRates => _scriptableObject.recollectableConfig.dropRates;

        private void Awake()
        {
            _currentHealth = _health;
        }

        private void DropItem(ItemObject item)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            while (x == 0 && y == 0)
            {
                x = Random.Range(-1f, 1f);
                y = Random.Range(-1f, 1f);
            }
            Vector2 dropDir = new Vector2(x, y).normalized;
            item.DropItem((dropDir * _dropForce + (Vector2)transform.position));
        }

        private ItemScriptableObject GetRandomItem()
        {
            int objectIndex = 0;
            if (_dropItems.Count > 1)
            {
                int probability = Random.Range(0, 101);
                for (int i = 0; i < _dropRates.Count; i++)
                {
                    if (probability <= _dropRates[i])
                    {
                        objectIndex = i;
                    }
                }
            }
            return _dropItems[objectIndex];
        }

        private void DestroyRecollectable()
        {
            Destroy(gameObject);
        }
        
        public void Recollect()
        {
            if (_dropItems.Count <= 0) return;

            GameObject tempDroppingObject = Instantiate(_dropItemPrefab, transform.position, Quaternion.identity);
            ItemObject tempDroppingItem = tempDroppingObject.GetComponent<ItemObject>();
            
            if (tempDroppingItem == null) return;
            tempDroppingItem.ObtainComponents();
            tempDroppingItem.SetItem(GetRandomItem());
            
            DropItem(tempDroppingItem);
            
            _currentHealth--;
            if (_currentHealth <= 0)
            {
                DestroyRecollectable();
            }
        }
    }
}
