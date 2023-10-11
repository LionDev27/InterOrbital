using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterOrbital.Combat.IA
{
    public class JellypusWakeUp : JellypusStateBase
    {
        public override void OnStateEnter()
        {
            _currentAgent.Animator.SetBool("Idle", false);
        }

        public override void Execute()
        {
            
        }
    }
}
