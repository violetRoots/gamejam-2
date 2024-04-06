using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExperienceCollectArea : MonoBehaviour
{
    private CrowdController _crowdController;

    private void Start()
    {
        _crowdController = CrowdController.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Experience experience)) return;

        Human human = null;
        var staticHumans = _crowdController.GetStaticHumanInfos().Select(info => info.human).ToArray();
        if(staticHumans.Length > 0)
        {
            human = staticHumans[Random.Range(0, staticHumans.Length)];
        }
        else
        {
            var rotatableHumans = _crowdController.GetRotatableHumanInfos().Select(info => info.human).ToArray();
            human = rotatableHumans[Random.Range(0, rotatableHumans.Length)];
        }

        experience.Init(human);
    }
}
