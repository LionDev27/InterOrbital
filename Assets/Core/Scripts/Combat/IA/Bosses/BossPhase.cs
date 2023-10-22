using UnityEngine;

namespace InterOrbital.Combat.IA
{
    [System.Serializable]
    public class BossPhase
    {
        public BossAttacks attacks;
        public int healthToChange;
        public Color barFillColor;
    }
}