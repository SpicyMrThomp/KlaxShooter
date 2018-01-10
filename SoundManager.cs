using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioSource soundEffect;
    public AudioSource gridSoundEffect;
    public AudioSource playerSoundEffect;
    public AudioSource music;
    protected float minPitch = .95f, maxPitch = 1.05f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayPlayerSound(AudioClip clip = null, float volume = 1)
    {
        PlaySound(playerSoundEffect, clip, volume);
    }

    public void PlayGridSound(AudioClip clip = null, float volume = 1)
    {
        PlaySound(gridSoundEffect, clip, volume);
    }

    public void PlaySoundEffect(AudioClip clip = null, float volume = 1)
    {
        PlaySound(soundEffect, clip, volume);
    }

    private void PlaySound(AudioSource source = null, AudioClip clip = null, float volume = 1)
    {
        if (volume > .1f)
        {
            source.clip = clip;
            source.pitch = Random.Range(minPitch, maxPitch);
            source.volume = volume;
            source.Play();
        }
    }

    public void StopCurrentSound(AudioClip clip)
    {
        if (soundEffect.isPlaying && SoundPlaying(clip))
        {
            soundEffect.Stop();
        }
    }

    public bool SoundPlaying(AudioClip clip)
    {
        if (soundEffect.clip == clip)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
