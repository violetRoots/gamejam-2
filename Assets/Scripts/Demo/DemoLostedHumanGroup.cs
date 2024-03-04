using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoLostedHumanGroup : MonoBehaviour
{
    public List<DemoHuman> LostedHumans => _lostedHumans;

    [SerializeField] private List<DemoHuman> _lostedHumans;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _lostedHumans = transform.parent.GetComponentsInChildren<DemoHuman>().ToList();
    }
#endif
}
