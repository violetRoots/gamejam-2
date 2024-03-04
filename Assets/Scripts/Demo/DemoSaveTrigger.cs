using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSaveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject oldHut;
    [SerializeField] private GameObject newHut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out DemoHuman human)) return;

        if (DemoCrowdController.Instance.SaveHuman(human))
        {
            oldHut.gameObject.SetActive(false);
            newHut.gameObject.SetActive(true);

            DemoUIManager.Instance.SetVisibleEndLevel(true);
            DemoAudioManager.Instance.SetEndLevelMusic();
        }
    }
}
