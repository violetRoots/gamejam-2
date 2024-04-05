using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : SingletonMonoBehaviourBase<PauseManager>
{
    public bool IsPaused { get; private set; }

    public void Pause()
    {
        IsPaused = true;

        Time.timeScale = 0;
    }

    public void UnPause()
    {
        IsPaused = false;

        Time.timeScale = 1;
    }
}
