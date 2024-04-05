using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ViewManager : SingletonFromResourcesBase<ViewManager>
{
    [SerializeField] private View[] views;

    [SerializeField] private Canvas canvas;

    private List<View> _instantiatedViews = new List<View>();

    public T Get<T>() where T : View
    {
        T instantiatedView = _instantiatedViews.OfType<T>().FirstOrDefault();
        if (instantiatedView == null)
            instantiatedView = Instantiate<T>();

        return instantiatedView;
    }

    private T Instantiate<T>() where T : View
    {
        var viewPrefab = views.OfType<T>().FirstOrDefault();
        var view = Instantiate(viewPrefab, canvas.transform);
        view.OnCreate();

        view.gameObject.SetActive(false);
        _instantiatedViews.Add(view);

        return view;
    }

    public T Show<T>() where T : View
    {
        var view = Get<T>();
        view.Show();

        return view;
    }

    public T Hide<T>() where T : View
    {
        var view = Get<T>();
        view.Hide();

        return view;
    }

    public bool IsShown<T>() where T : View
    {
        return Get<T>().IsShown();
    }
}
