using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject oldHut;
    [SerializeField] private GameObject newHut;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Human human)) return;

        if (CrowdController.Instance.SaveHuman(human))
        {
            oldHut.gameObject.SetActive(false);
            newHut.gameObject.SetActive(true);

            AudioManager.Instance.SetEndLevelMusic();
        }
    }
}
