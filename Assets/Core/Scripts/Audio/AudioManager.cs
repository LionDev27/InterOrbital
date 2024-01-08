using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource, sfxSource;
    public AudioMixer mixer;
    public Sound[] musicSounds, sfxSounds;

    private float _musicVolume;
    private bool _muted;
    private float _defaultMainVolume;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _musicVolume = musicSource.volume;
        if (SceneManager.GetActiveScene().name == "Game")
            PlayMusic("MainTheme", true);
        mixer.GetFloat("MasterVol", out var vol);
        _defaultMainVolume = vol;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mixer.SetFloat("MasterVol", _muted ? _defaultMainVolume : -80f);
            _muted = !_muted;
        }
    }

    public void PlayMusic(string name, bool loop)
    {
        Sound sound = Array.Find(musicSounds, s => s.name == name);

        if (sound != null)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(musicSource.DOFade(0f, 1f).OnComplete(() =>
            {
                musicSource.clip = sound.clip;
                musicSource.Play();
            }));
            sequence.Append(musicSource.DOFade(_musicVolume, 1f));
            sequence.Play();

            musicSource.loop = loop;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, s => s.name == name);

        if (sound != null)
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    private IEnumerator PlaySoundEvery(string name, float t, int times)
    {
        Sound sound = Array.Find(musicSounds, s => s.name == name);

        for (int i = 0; i < times; i++)
        {
            PlaySFX(name);
            yield return new WaitForSeconds(t);
        }
    }

    public void ModifySFXVolume(float db)
    {
        mixer.DOSetFloat("SFXVol", db, 1f);
    }

    public void ModifyMusicVolume(float db)
    {
        mixer.DOSetFloat("MusicVol", db,2f);
    }
}