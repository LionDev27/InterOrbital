using System;
using InterOrbital.Item;

namespace InterOrbital.Utils
{
    [Serializable]
    public struct ItemRequired
    {
        public ItemScriptableObject item;
        public int amountRequired;
    }
}
