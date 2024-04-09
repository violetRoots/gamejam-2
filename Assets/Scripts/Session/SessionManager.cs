using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : SingletonMonoBehaviourBase<SessionManager>
{
    public class SessionInfo
    {
        public TimeSpan time;
    }

    [SerializeField] private float completeSessionTime = 5.0f;

    private float _startSessionTime;
    private SessionInfo _sessionInfo = new SessionInfo();

    private SessionTimerPanel _timerPanel;

    private void Start()
    {
        _timerPanel = ViewManager.Instance.Get<SessionTimerPanel>();

        StartSession();
    }

    private void Update()
    {
        UpdateSessionTime();
    }

    private void UpdateSessionTime()
    {
        _sessionInfo.time = TimeSpan.FromSeconds(Time.time - _startSessionTime);

    }

    private void StartSession()
    {
        _startSessionTime = Time.time;
    }

    public void EndSession()
    {

    }
}
