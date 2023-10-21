using InterOrbital.Utils;

namespace InterOrbital.Combat.IA
{
    public class JellypusTentacleFlipper : TargetDirFlipper
    {
        private JellypusTentacleAttack _attack;

        private void Awake()
        {
            _attack = GetComponentInParent<JellypusTentacleAttack>();
        }

        private void OnEnable()
        {
            target = _attack.Agent.Target;
        }
    }
}