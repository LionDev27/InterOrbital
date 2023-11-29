using System.Collections.Generic;
using InterOrbital.Others;
using InterOrbital.Player;
using InterOrbital.UI;
using UnityEngine;

namespace InterOrbital.Mission
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField] private List<MissionScriptableObject> missionsToDo;
        private int _indexActualMission;
        private MissionCreator _missionCreator;
        public static MissionManager Instance = null;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            _missionCreator = FindObjectOfType<MissionCreator>();
            _indexActualMission = -1;
        }

        private void Start()
        {
            NextMission();    
        }

        public void NextMission()
        {
            _indexActualMission++;
            if (_indexActualMission < missionsToDo.Count)
            {
                _missionCreator.CreateMission(missionsToDo[_indexActualMission]);
                if (_indexActualMission != missionsToDo.Count - 1) return;
                for (int i = 0; i < UIManager.Instance.RemainingEnergyTiers; i++)
                    PlayerComponents.Instance.PlayerEnergy.UpgradeEnergy(20);
                for (int i = 0; i < UIManager.Instance.RemainingLifeTiers; i++)
                    PlayerComponents.Instance.PlayerDamageable.UpgradeHealth(8);
            }
            else
            {
                PlayerComponents.Instance.InputHandler.DeactivateControls();
                LevelManager.Instance.BackMenu();
            }
            
        }

    }
}
   
