using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Item
{
    public class DeathBagInteractable : BaseInteractable
    {
        [SerializeField] private DeathBag _deathBag;
        public override void Interact()
        {
            _deathBag.FillPlayerInventory();
        }

        public override void EndInteraction()
        {
        }
    }
}

