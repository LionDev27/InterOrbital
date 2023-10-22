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
            _currentAgent.Damageable.ActivateBoss();
        }

        public override void Execute()
        {
            if (GetCurrentClipName() == "JellypusAlert")
                _currentAgent.ChangeState(_currentAgent.States[2]);
        }
        
        private string GetCurrentClipName()
        {
            return _currentAgent.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        }
    }
}
