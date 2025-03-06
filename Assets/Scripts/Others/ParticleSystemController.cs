using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private ParticleSystem currentParticleSystem;

    void Start()
    {
        currentParticleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Check if the particle system is not playing anymore
        if (!currentParticleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
