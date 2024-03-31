using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : Enemy
{
    [SerializeField] private StaticHuman staticHumanPrefab;

    public override void Die()
    {
        var staticHuman = Instantiate(staticHumanPrefab, transform.position, Quaternion.identity);
        _crowdController.AddHuman(staticHuman);

        base.Die();
    }
}
