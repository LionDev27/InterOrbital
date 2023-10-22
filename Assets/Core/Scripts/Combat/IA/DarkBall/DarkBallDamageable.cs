using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InterOrbital.Combat.IA
{
    public class DarkBallDamageable : EnemyDamageable
    {
        [SerializeField] private DarkBall _darkBall;
        protected override void Death()
        {
            _darkBall.DeathDarkBall();
        }
    }
}


