using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InterOrbital.Item;

namespace InterOrbital.Mission
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MissionItemScriptableObject", order = 0)]
    public class MissionItemScriptableObject : MissionScriptableObject
    {
        public ItemScriptableObject itemGoal;
    }
}
   
