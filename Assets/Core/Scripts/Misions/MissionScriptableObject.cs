using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Utils;
using UnityEngine.UI;

namespace InterOrbital.Mission
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MissionScriptableObject", order = 0)]
    public class MissionScriptableObject : ScriptableObject
    {
        public TypeMission typeMission;
        public string missionDescription;
        public int amountToReach;
        public Sprite imageMission;
    }
}

