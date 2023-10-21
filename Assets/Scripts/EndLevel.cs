using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI saved;
    [SerializeField] private TextMeshProUGUI died;
    [SerializeField] private TextMeshProUGUI killed;

    private void Update()
    {
        var crowd = CrowdController.Instance;

        saved.text = $"�������: {crowd.SavedCount}";
        died.text = $"�������: {crowd.DiedCount}";
        killed.text = $"������� � �����: {crowd.KilledCount}";
    }
}
