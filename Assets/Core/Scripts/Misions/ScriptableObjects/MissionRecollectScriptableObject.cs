using UnityEngine;

namespace InterOrbital.Mission
{
    [CreateAssetMenu(fileName = "RecollectMission", menuName = "ScriptableObjects/Missions/RecollectMission", order = 0)]
    public class MissionRecollectScriptableObject : MissionScriptableObject
    {
        public int amountToReach;
    }
}