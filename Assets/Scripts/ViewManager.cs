using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;

public class ViewManager : SingletonMonoBehaviourBase<ViewManager>
{
    [SerializeField] private View[] views;

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
        var view = Instantiate(viewPrefab, transform);
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
