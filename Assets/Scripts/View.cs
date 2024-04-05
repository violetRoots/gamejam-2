using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class View : MonoBehaviour
{
    [Header("View")]
    [SerializeField] private bool useAnimation;

    [ShowIf(nameof(useAnimation))]
    [SerializeField] private float fadeTime = 0.1f;

    public CanvasGroup BaseGroup
    {
        get
        {
            if (_baseGroup == null)
                _baseGroup = GetComponent<CanvasGroup>();
            return _baseGroup;
        }
    }

    private CanvasGroup _baseGroup;

    public virtual void OnCreate()
    {

    }

    public void Show()
    {
        if (useAnimation)
            ShowAnimated();
        else
            ShowImmediate();
    }

    protected virtual void OnShow()
    {

    }

    public void ShowImmediate()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public void ShowAnimated()
    {
        gameObject.SetActive(true);
        BaseGroup.alpha = 0.0f;
        BaseGroup.DOFade(1.0f, fadeTime).OnComplete(OnShow);
    }

    public void Hide()
    {
        if (useAnimation)
            HideAnimated();
        else
            HideImmediate();
    }

    protected virtual void OnHide()
    {

    }

    public void HideImmediate()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    public void HideAnimated()
    {
        BaseGroup.DOFade(0.0f, fadeTime).OnComplete(() =>
        {
            OnHide();
            gameObject.SetActive(false);
        });
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }
}
