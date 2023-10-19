using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LostedHumanGroup : MonoBehaviour
{
    public List<Human> LostedHumans => _lostedHumans;

    [SerializeField] private List<Human> _lostedHumans;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _lostedHumans = transform.parent.GetComponentsInChildren<Human>().ToList();
    }
#endif
}
