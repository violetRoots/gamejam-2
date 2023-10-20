using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shooter : Human
{
    [Space]
    [SerializeField] private float bulletsSpeed = 10.0f;
    [SerializeField] private List<BodySpriteInfo> bodySprites;
     
    [Space]
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform body;
    [SerializeField] private SpriteRenderer bodySprite;

    [Space]
    [SerializeField] private Bullet bullet;

    private void OnDisable()
    {
        if (InputManager.Instance == null) return;

        InputManager.Instance.OnLeftMouseButtonDown -= Shoot;
    }

    public override void AddInCrowd()
    {
        base.AddInCrowd();

        InputManager.Instance.OnLeftMouseButtonDown += Shoot;
    }

    private void Shoot()
    {
        var newBullet = Instantiate(bullet, bulletOrigin.position, Quaternion.identity);
        newBullet.Init(body.right, bulletsSpeed);

        AudioManager.Instance.PlayShootSound();
    }

    public void SetBodyRotation(Vector3 rotationAngles)
    {
        body.rotation = Quaternion.Euler(rotationAngles);

        //UpdateBodySprite(rotationAngles);
    }

    private void UpdateBodySprite(Vector3 rotationAngles)
    {
        Debug.Log(rotationAngles.z);

        var bodySpriteInfo = bodySprites.OrderBy(b => Mathf.Abs(rotationAngles.z - b.angle)).First();
        bodySprite.sprite = bodySpriteInfo.sprite;
    }
}

[Serializable]
public class BodySpriteInfo
{
    public float angle;
    public Sprite sprite;
}
