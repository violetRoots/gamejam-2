using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private ViewManager _viewManager;

    private void Awake()
    {
        _viewManager = ViewManager.Instance;

        _viewManager.Show<MainMenuPanel>();
    }
}
