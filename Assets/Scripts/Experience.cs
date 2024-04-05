using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    public int Points => startExperiencePoints;

    [SerializeField] private int startExperiencePoints = 10;

    [Space]
    [SerializeField] private float lerpSpeed = 1.0f;
    [SerializeField] private Vector2 endPositionOffset = Vector2.up;
    [SerializeField] private float endScale = 1.0f;

    private LevelManager _levelManager;

    private Human _targetHuman;

    private void Start()
    {
        _levelManager = LevelManager.Instance;
    }

    public void Init(Human target)
    {
        _targetHuman = target;

        StartCoroutine(MoveProcess());
    }

    private IEnumerator MoveProcess()
    {
        yield return LerpPositionAndScale(_targetHuman.transform,
                                          endPositionOffset,
                                          endScale,
                                          lerpSpeed);


        _levelManager.AddExperiencePoints(Points);
        if(_targetHuman != null && !_targetHuman.IsDied())
            _targetHuman.AddExperiencePoints(Points);

        Destroy(gameObject);
    }

    private IEnumerator LerpPositionAndScale(Transform target, Vector2 endPosOffset, float endScale, float speed)
    {
        float lerpValue = 0.0f;

        var startPos = transform.position;

        var startScaleVector = transform.localScale;
        var endScaleVector = new Vector3(endScale, endScale, 1.0f);

        while (lerpValue < 1.0f)
        {
            lerpValue += speed * Time.smoothDeltaTime;

            if (target == null)
                yield break;

            var endPos = target.position + (Vector3)endPosOffset;

            transform.position = Vector2.Lerp(startPos, endPos, lerpValue);
            transform.localScale = Vector2.Lerp(startScaleVector, endScaleVector, lerpValue);
            yield return null;
        }
    }
}
