using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCrowdInviteArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out DemoLostedHumanGroup lostedHumanGroup)) return;

        foreach (var human in lostedHumanGroup.LostedHumans)
        {
            if(human == null) continue;

            DemoCrowdController.Instance.AddHuman(human);
        }

        Destroy(lostedHumanGroup.gameObject);
    }
}
