using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Utils;
using UnityEngine.UI;

namespace InterOrbital.Mission
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Missions/Mission", order = 0)]
    public class MissionScriptableObject : ScriptableObject
    {
        public TypeMission typeMission;
        public string missionDescription;
        public string nextConversationName;
        public Sprite imageMission;
    }
}

