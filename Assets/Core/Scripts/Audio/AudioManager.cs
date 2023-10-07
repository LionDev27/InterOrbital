using System;
using System.Collections;
using UnityEngine;

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
    public Sound[] musicSounds, sfxSounds;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        /*else
        {
            Destroy(gameObject);
        }*/

    }

    private void Start()
    {
        PlayMusic("MainTheme",true);
    }

    public void PlayMusic(string name,bool loop)
    {
        Sound sound = Array.Find(musicSounds, s => s.name == name);

        if (sound != null)
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
            if (loop)
            {
                musicSource.loop = true;
            }
            else
            {
                musicSource.loop = false;
            }
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
        Sound sound = Array.Find(musicSounds, s => s.name == name);

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
}
