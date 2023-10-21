using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private float startTime = 20.0f;

    private ParticleSystem _system;

    private void Start()
    {
        _system = GetComponent<ParticleSystem>();
        _system.time = startTime;
    }
}
