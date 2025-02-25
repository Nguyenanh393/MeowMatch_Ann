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

    private bool _isSoundOn = true;
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
        if (_isSoundOn) buttonSoundSource.Play();
    }

    public void PlayScoreSound()
    {
        if (_isSoundOn) scoreSoundSource.Play();
    }

    public void PlayWinSound()
    {
        if (_isSoundOn) winSoundSource.Play();
    }

    public void PlayLoseSound()
    {
        if (_isSoundOn) loseSoundSource.Play();
    }

    public void ToggleSound(bool isOn)
    {
        _isSoundOn = isOn;

        // Bật/tắt âm lượng cho toàn bộ AudioSources
        var volume = _isSoundOn ? 1f : 0f;
        backgroundMusicSource.volume = volume;
        buttonSoundSource.volume = volume;
        scoreSoundSource.volume = volume;
        winSoundSource.volume = volume;
        loseSoundSource.volume = volume;
    }

    public bool IsSoundOn()
    {
        return _isSoundOn;
    }
}

