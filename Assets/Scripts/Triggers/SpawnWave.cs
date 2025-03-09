using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 // Add this line for Light2D

public class SpawnWave : MonoBehaviour
{
    public GateController gateController;
    public WaveSpawner waveSpawner;
    public List<UnityEngine.Rendering.Universal.Light2D> lightsToTurnOff;
    
    public List<UnityEngine.Rendering.Universal.Light2D> lightsToTurnOn;
    private Collider2D triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        foreach (UnityEngine.Rendering.Universal.Light2D light in lightsToTurnOn)
        {
            light.enabled = false;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            gateController.OpenGate();
            waveSpawner.canSpawn = true;
            triggerCollider.enabled = false;

            foreach (UnityEngine.Rendering.Universal.Light2D light in lightsToTurnOff)
            {
                light.enabled = false;
            }

            foreach (UnityEngine.Rendering.Universal.Light2D light in lightsToTurnOn)
            {
                light.enabled = true;
            }
        }
    }
}