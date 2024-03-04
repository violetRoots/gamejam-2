using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoShooter : DemoHuman
{
    [Space]
    [SerializeField] private float bulletsSpeed = 10.0f;
    [SerializeField] private List<BodySpriteInfo> bodySprites;
     
    [Space]
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform body;
    [SerializeField] private SpriteRenderer bodySprite;

    [Space]
    [SerializeField] private DemoBullet bullet;

    private void OnDisable()
    {
        if (DemoInputManager.Instance == null) return;

        DemoInputManager.Instance.OnLeftMouseButtonDown -= Shoot;
    }

    public override void AddInCrowd()
    {
        base.AddInCrowd();

        DemoInputManager.Instance.OnLeftMouseButtonDown += Shoot;
    }

    private void Shoot()
    {
        var newBullet = Instantiate(bullet, bulletOrigin.position, Quaternion.identity);
        newBullet.Init(body.right, bulletsSpeed);

        DemoAudioManager.Instance.PlayShootSound();
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
