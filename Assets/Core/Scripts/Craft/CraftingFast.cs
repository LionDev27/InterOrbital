using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InterOrbital.Item
{
    public class CraftingFast : CraftingItem
    {
        protected override void Start()
        {
            _craftUI = gameObject;
            base.Start();
        }

    }
}

