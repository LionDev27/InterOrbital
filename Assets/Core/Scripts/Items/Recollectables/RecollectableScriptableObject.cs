using UnityEngine;

namespace InterOrbital.Recollectables
{
    [CreateAssetMenu(fileName = "New Recollectable", menuName = "ScriptableObjects/Recollectable")]
    public class RecollectableScriptableObject : ScriptableObject
    {
        public RecollectableConfig recollectableConfig;
    }
}