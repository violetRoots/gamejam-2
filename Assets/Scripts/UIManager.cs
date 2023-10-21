using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : SingletonMonoBehaviourBase<UIManager>
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
        InputManager.Instance.OnEcsButtonDown += SwitchPause;
    }

    private void OnDisable()
    {
        if (InputManager.Instance == null) return;

        InputManager.Instance.OnEcsButtonDown -= SwitchPause;
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

        AudioManager.Instance.SetMusicVolume(value ? 0.5f : 1.0f);
    }

    public void SetVisibleGameOver(bool value)
    {
        SetGameTime(value);
        gameOver.SetActive(value);

        if(value)
            AudioManager.Instance.PlayGameOverSound();

        UpdateMusic(value);
    }

    public void SetVisibleEndLevel(bool value)
    {
        endLevel.gameObject.SetActive(value);
    }

    public void SetGameTime(bool value)
    {
        Time.timeScale = value ? 0.0f : 1.0f;
        InputManager.Instance.GameplayInputEnabled = !value;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SwitchPause()
    {
        var value = !pause.gameObject.activeSelf;

        if (value && !InputManager.Instance.GameplayInputEnabled) return;

        SetVisiblePause(value);
    }

    private void UpdateMusic(bool value)
    {
        if (value)
            AudioManager.Instance.SetMenuMusic();
        else
            AudioManager.Instance.SetGameplayMusic();
    }
}
