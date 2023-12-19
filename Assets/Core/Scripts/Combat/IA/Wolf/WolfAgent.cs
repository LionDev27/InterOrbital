using UnityEngine;

namespace InterOrbital.Combat.IA.Wolf
{
    public class WolfAgent : EnemyAgentBase
    {
        private SpriteFlipper _spriteFlipper;
        public SpriteFlipper SpriteFlipper => _spriteFlipper;

        protected override void Awake()
        {
            base.Awake();
            _spriteFlipper = GetComponentInChildren<SpriteFlipper>();
        }

        protected override void Update()
        {
            base.Update();
        }
        
        public void FlipSprite()
        {
            if (Target == null) return;
            switch (Mathf.Sign(Target.transform.position.x - transform.position.x))
            {
                case > 0:
                    SpriteFlipper.FlipX(1);
                    break;
                case < 0:
                    SpriteFlipper.FlipX(0);
                    break;
            }
        }
    }
}