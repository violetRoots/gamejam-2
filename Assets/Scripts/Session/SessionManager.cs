using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : SingletonMonoBehaviourBase<SessionManager>
{
    private ViewManager _viewManager;

    private void Awake()
    {
        _viewManager = ViewManager.Instance;

        _viewManager.Show<SessionProgressPanel>();
    }

    private void StartSession()
    {
        
    }

    public void EndSession()
    {

    }
}
