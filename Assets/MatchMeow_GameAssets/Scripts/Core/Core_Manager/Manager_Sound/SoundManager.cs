using System;
using UnityEngine;

public class SoundManager :Singleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource buttonSoundSource;
    [SerializeField] private AudioSource scoreSoundSource;
    [SerializeField] private AudioSource winSoundSource;
    [SerializeField] private AudioSource loseSoundSource;

    private void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlayButtonSound()
    {
        buttonSoundSource.Play();
    }

    public void PlayScoreSound()
    {
        scoreSoundSource.Play();
    }

    public void PlayWinSound()
    {
        winSoundSource.Play();
    }

    public void PlayLoseSound()
    {
        loseSoundSource.Play();
    }
}

