using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Item;
using UnityEngine.Serialization;

namespace InterOrbital.Mission
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Missions/ItemMission", order = 0)]
    public class MissionItemScriptableObject : MissionRecollectScriptableObject
    {
        public List<ItemScriptableObject> itemsGoalList;
    }
}
   
