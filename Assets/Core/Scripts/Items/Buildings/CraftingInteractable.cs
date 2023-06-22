using System;

namespace InterOrbital.Item
{
    public class CraftingInteractable : BaseInteractable
    {
        private CraftingTable _craftingTable;

        private void Awake()
        {
            _craftingTable = GetComponent<CraftingTable>();
        }

        public override void Interact()
        {
            _craftingTable.Interact();
        }

        public override void EndInteraction()
        {
            _craftingTable.EndInteraction();
        }
    }
}