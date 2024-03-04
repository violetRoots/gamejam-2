using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoUIManager : SingletonMonoBehaviourBase<DemoUIManager>
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject endLevel;

    private void Start()
    {
        SetVisiblePause(false);
        SetVisibleGameOver(false);
        SetVisibleMenu(true);
    }

    private void OnEnable()
    {
        DemoInputManager.Instance.OnEcsButtonDown += SwitchPause;
    }

    private void OnDisable()
    {
        if (DemoInputManager.Instance == null) return;

        DemoInputManager.Instance.OnEcsButtonDown -= SwitchPause;
    }

    public void SetVisibleMenu(bool value)
    {
        SetGameTime(value);
        menu.SetActive(value);

        UpdateMusic(value);
    }

    public void SetVisiblePause(bool value)
    {
        SetGameTime(value);
        pause.SetActive(value);

        DemoAudioManager.Instance.SetMusicVolume(value ? 0.5f : 1.0f);
    }

    public void SetVisibleGameOver(bool value)
    {
        SetGameTime(value);
        gameOver.SetActive(value);

        if(value)
            DemoAudioManager.Instance.PlayGameOverSound();

        UpdateMusic(value);
    }

    public void SetVisibleEndLevel(bool value)
    {
        endLevel.gameObject.SetActive(value);
    }

    public void SetGameTime(bool value)
    {
        Time.timeScale = value ? 0.0f : 1.0f;
        DemoInputManager.Instance.GameplayInputEnabled = !value;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SwitchPause()
    {
        var value = !pause.gameObject.activeSelf;

        if (value && !DemoInputManager.Instance.GameplayInputEnabled) return;

        SetVisiblePause(value);
    }

    private void UpdateMusic(bool value)
    {
        if (value)
            DemoAudioManager.Instance.SetMenuMusic();
        else
            DemoAudioManager.Instance.SetGameplayMusic();
    }
}
