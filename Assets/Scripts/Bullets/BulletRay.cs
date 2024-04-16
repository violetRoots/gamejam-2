using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRay : BaseProjectile
{
    [SerializeField] private float fillSpeed = 1.0f;

    [SerializeField] private Transform filler;

    private Vector3 _startSize;

    public void Init(Vector2 direction, int damage)
    {
        SetInitTime();
        SetDamage(damage);

        transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.SignedAngle(Vector2.right, direction));

        _startSize = filler.localScale;
        filler.localScale = new Vector3(0.0f, _startSize.y, 1.0f);

        StartCoroutine(FillProcess());
    }

    private IEnumerator FillProcess()
    {
        var lerpCount = 0.0f;
        while (lerpCount <= 1.0f)
        {
            filler.localScale = new Vector3(Mathf.Lerp(0.0f, _startSize.x, _startSize.y), 1.0f);

            yield return null;

            lerpCount += fillSpeed * Time.smoothDeltaTime;
        }
    }
}
