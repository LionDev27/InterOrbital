using InterOrbital.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InterOrbital.Spaceship
{
    public class SpaceshipComponents : MonoBehaviour
    {
        public SpaceshipEnergy SpaceshipEnergy { get; private set; }

        public static SpaceshipComponents Instance;


        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this;

            SpaceshipEnergy = GetComponent<SpaceshipEnergy>();
        }
    }
}