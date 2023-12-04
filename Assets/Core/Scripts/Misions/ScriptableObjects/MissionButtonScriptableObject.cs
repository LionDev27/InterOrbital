using UnityEngine;

namespace InterOrbital.Mission
{
    [CreateAssetMenu(fileName = "ButtonMission", menuName = "ScriptableObjects/Missions/ButtonMission", order = 0)]
    public class MissionButtonScriptableObject : MissionScriptableObject
    {
        public KeyCode[] buttons;
    }
}