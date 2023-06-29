using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Mission
{
    public class MissionManager : MonoBehaviour
    {
        [SerializeField] private List<MissionScriptableObject> missionsToDo;
        private int _indexActualMission;
        private  MissionCreator _missionCreator;
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
            }
            else
            {
                //FIN DEMO
            }
            
        }

    }
}
   
