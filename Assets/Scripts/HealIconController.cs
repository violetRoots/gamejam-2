using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealIconController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float stopDistance = 0.1f;

    [Space]
    [SerializeField] private float sourceLerpSpeed = 1.0f;
    [SerializeField] private Vector2 sourceEndPositionOffset = Vector2.up;
    [SerializeField] private float sourceEndScale = 1.0f;

    [Space]
    [SerializeField] private float targetLerpSpeed = 1.0f;
    [SerializeField] private Vector2 targetEndPositionOffset = Vector2.up;
    [SerializeField] private float targetEndScale = 1.0f;

    private Transform _source;
    private Transform _target;

    public void Init(Transform source, Transform target)
    {
        _source = source;
        _target = target;

        StartCoroutine(MoveProcess());
    }

    private IEnumerator MoveProcess()
    {
        yield return LerpPositionAndScale(_source, 
                                          sourceEndPositionOffset,
                                          sourceEndScale,
                                          sourceLerpSpeed);

        yield return LerpPositionAndScale(_target,
                                          targetEndPositionOffset,
                                          targetEndScale,
                                          targetLerpSpeed);

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

            var endPos = target.position + (Vector3) endPosOffset;

            transform.position = Vector2.Lerp(startPos, endPos, lerpValue);
            transform.localScale = Vector2.Lerp(startScaleVector, endScaleVector, lerpValue);
            yield return null;
        }
    }
}
