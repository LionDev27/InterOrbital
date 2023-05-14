using InterOrbital.UI;
using InterOrbital.Utils;
using UnityEngine;

namespace InterOrbital.Item
{
    public class StorageItem : MonoBehaviour, IInteractable
    {
        private GameObject _storageUI;

        private void Start()
        {
            _storageUI = UIManager.Instance.storageUI;
        }

        public void Interact()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_storageUI);
        }

        public void EndInteraction()
        {
            UIManager.Instance.ActivateOrDesactivateUI(_storageUI);
        }
    }
}
