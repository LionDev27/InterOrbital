using System;
using UnityEngine;

namespace InterOrbital.Item
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        [SerializeField] private GameObject _interactVisuals;

        protected virtual void Start()
        {
            if (_interactVisuals.activeInHierarchy)
                _interactVisuals.SetActive(false);
        }

        public virtual void ShowInteraction(bool value)
        {
            if (_interactVisuals.activeInHierarchy != value)
                _interactVisuals.SetActive(value);
        }

        public abstract void Interact();

        public abstract void EndInteraction();
    }
}