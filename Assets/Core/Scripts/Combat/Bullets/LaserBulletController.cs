using InterOrbital.Combat;
using InterOrbital.Combat.Bullets;
using System.Collections;
using UnityEngine;

public class LaserBulletController : BaseBulletController
{
    private float _timerToBringLaser = 1f;
    private float _laserDuration = 2f;
    private bool _laserActivated;
    [SerializeField] private Collider2D _laserCollider;
    [SerializeField] private GameObject _placeholder;

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
            _placeholder.SetActive(true);
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
