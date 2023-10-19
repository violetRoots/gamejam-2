using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdInviteArea : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out LostedHumanGroup lostedHumanGroup)) return;

        foreach (var human in lostedHumanGroup.LostedHumans)
            CrowdController.Instance.AddHuman(human);

        Destroy(lostedHumanGroup.gameObject);
    }
}
