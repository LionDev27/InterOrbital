using InterOrbital.Events;
using InterOrbital.Player;
using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorrentialRains_Event : EventBase
{
    [SerializeField] private ParticleSystem _rainEffect;
    [SerializeField] private ParticleSystem _splashEffect;
    [SerializeField] private GameObject _waterfall;
    [SerializeField] private float _radius;
    [SerializeField] private float _firstTimeToSpawn;
    private float _timeToSpawn;
     private bool _eventIsActive;
    private int _numWaterFalls;
    public override void StartEvent()
    {
        Rain();
        _eventIsActive = true;
        _numWaterFalls = 1;
        _timeToSpawn = _firstTimeToSpawn;
        StartCoroutine(GenerateFalls());
        AudioManager.Instance.ModifyMusicVolume(-10);
        AudioManager.Instance.PlayMusic("EventMusic1",true);
    }

    public override void EndEvent()
    {
        _rainEffect.Stop();
        _splashEffect.Stop();
        _eventIsActive = false;
        StopCoroutine(GenerateFalls());
        AudioManager.Instance.ModifyMusicVolume(10);
        AudioManager.Instance.StopAmbientSFX();
        AudioManager.Instance.PlayMusic("MainTheme", true);
    }


    private IEnumerator GenerateFalls()
    {
        int numWaves = 0;
        while (_eventIsActive)
        {
            yield return new WaitForSeconds(_timeToSpawn);

            if (_eventIsActive && EventsManager.Instance.currentTime >= 3)
            {
                for(int i=0; i< _numWaterFalls; i++)
                {
                    Vector3 spawnPosition;
                    do
                    {
                        yield return new WaitForSeconds(0.5f);
                        Vector2 centerPosition = PlayerComponents.Instance.GetPlayerPosition();
                        float randomAngle = Random.Range(0f, 2 * Mathf.PI);
                        float x = centerPosition.x + _radius * Mathf.Cos(randomAngle);
                        float y = centerPosition.y + _radius * Mathf.Sin(randomAngle);
                        spawnPosition = new Vector3(x, y, 0);
                    } while (!(spawnPosition.x >= 0 && spawnPosition.x < GridLogic.Instance.width && spawnPosition.y >= 0 && spawnPosition.y < GridLogic.Instance.height));

                    Instantiate(_waterfall, spawnPosition, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("Waterfall");
                }
                numWaves++;
            }

            if (numWaves == 2)
            {
                _timeToSpawn -= 1;
                _numWaterFalls++;
            }
            else if (numWaves == 4)
            {
                _timeToSpawn -= 1;
                _numWaterFalls++;
            }
            else if (numWaves == 6)
            {
                _timeToSpawn -= 1;
                _numWaterFalls++;
            }
            else if (numWaves == 9)
            {
                _numWaterFalls++;
            }

        }
    }

    public void Rain()
    {
        _rainEffect.Simulate(10, true, false);
        _rainEffect.Play();
        _splashEffect.Play();
        AudioManager.Instance.PlayAmbientSFX("Rain", true);
    }

    private void OnDrawGizmos()
    {
        if (_eventIsActive)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(PlayerComponents.Instance.GetPlayerPosition(), _radius); 
        }
    }
}
