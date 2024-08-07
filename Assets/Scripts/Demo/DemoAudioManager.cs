using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAudioManager : SingletonMonoBehaviourBase<DemoAudioManager>
{
    [SerializeField] private AudioClip gameplayClip;
    [SerializeField] private AudioClip menuClip;
    [SerializeField] private AudioClip endLevelClip;

    [Space]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource shootSource;
    [SerializeField] private AudioSource enemySource;
    [SerializeField] private AudioSource humanSource;
    [SerializeField] private AudioSource gameOverSource;

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

    public void PlayGameOverSound()
    {
        gameOverSource.Play();
    }

    private void ChangeMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
