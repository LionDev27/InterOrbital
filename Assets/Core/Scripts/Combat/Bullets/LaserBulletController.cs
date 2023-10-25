using InterOrbital.Combat;
using InterOrbital.Combat.Bullets;
using System.Collections;
using UnityEngine;

public class LaserBulletController : BaseBulletController
{
    private float _timerToBringLaser = 0.45f;
    private float _laserDuration = 1.05f;
    private bool _laserActivated;
    [SerializeField] private Collider2D _laserCollider;

    private new void Update()
    {
        base.Update();
        GenerateLaser();
    }

    private void GenerateLaser()
    {
        if(_timerToBringLaser > 0) 
        {
            _timerToBringLaser -= Time.deltaTime;
        }

        if(_timerToBringLaser <= 0 && !_laserActivated)
        {
            _laserActivated = true;
            _laserCollider.enabled = true;
            StopMove();
            GetComponent<BulletDD>().DontDestroyAfterHit();
            StartCoroutine(FinishLaser());
        }
    }

    private IEnumerator FinishLaser()
    {
        yield return new WaitForSeconds(_laserDuration);
        Destroy(gameObject);
    }
}
