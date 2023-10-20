using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviourBase<AudioManager>
{
    [SerializeField] private AudioClip gameplayClip;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip endLevelClip;

    [Space]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource shootSource;
    [SerializeField] private AudioSource enemySource;
    [SerializeField] private AudioSource humanSource;

    public void SetGameplayMusic()
    {
        ChangeMusic(gameplayClip);
    }

    public void SetMenuMusic()
    {
        ChangeMusic(menuClip);
    }

    public void SetEndLevelMusic()
    {
        ChangeMusic(endLevelClip);
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void PlayShootSound()
    {
        shootSource.Play();
    }

    public void PlayHumanSound()
    {
        humanSource.Play();
    }

    public void PlayEnemySound()
    {
        enemySource.Play();
    }

    private void ChangeMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
