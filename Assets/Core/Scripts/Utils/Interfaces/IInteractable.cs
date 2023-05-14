using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Utils
{
    public interface IInteractable
    {
        public void Interact();

        public void EndInteraction();
    }
}
