using InterOrbital.Combat.Bullets;
using UnityEngine;

public class ClusterBulletController : BaseBulletController
{
    private float _timerToBroke = 0.5f;
    [SerializeField] private GameObject _miniClusterBulletPrefab;


    private new void Update()
    {
        base.Update();
        GenerateCluster();
    }

    private void GenerateCluster()
    {
        if(_timerToBroke > 0) 
        {
            _timerToBroke -= Time.deltaTime;
        }

        if(_timerToBroke <= 0)
        {
            InstantiateClusterBullet(0);
            InstantiateClusterBullet(45);
            InstantiateClusterBullet(-45);

            Destroy(gameObject);
        }
    }

    private void InstantiateClusterBullet(float angle)
    {
        var tempBullet = Instantiate(_miniClusterBulletPrefab, transform.position, Quaternion.identity);
        var bulletController = tempBullet.GetComponent<BaseBulletController>();
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        Vector3 direction3D = new Vector3(_moveDir.x, _moveDir.y, 0f);
        Vector3 rotatedDirection = rotation * direction3D;

        var bulletMoveDir = new Vector2(rotatedDirection.x, rotatedDirection.y);
        bulletController.SetupBullet(_damageDealer.attackerTag,bulletMoveDir, transform.position);
    }
}
