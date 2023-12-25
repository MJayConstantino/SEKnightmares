using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 // Add this line for Light2D

public class SpawnWave : MonoBehaviour
{
    public GateController gateController; // Reference to the gate controller script
    public WaveSpawner waveSpawner;
    public List<UnityEngine.Rendering.Universal.Light2D> lightsToTurnOff;
    
    public List<UnityEngine.Rendering.Universal.Light2D> lightsToTurnOn; // List of Light2D components to be turned off
    private Collider2D triggerCollider; // Reference to the trigger collider

    // Start is called before the first frame update
    void Start()
    {

        // Get the reference to the Collider component
        triggerCollider = GetComponent<Collider2D>();
        foreach (UnityEngine.Rendering.Universal.Light2D light in lightsToTurnOn)
        {
            light.enabled = false;
        }

    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            // Enable the gate controller
            gateController.OpenGate();
            
            // Set canSpawn to true in the WaveSpawner script
            waveSpawner.canSpawn = true;
            
            // Disable the trigger collider
            triggerCollider.enabled = false;

            // Turn off the lights
            foreach (UnityEngine.Rendering.Universal.Light2D light in lightsToTurnOff)
            {
                light.enabled = false;
            }

            foreach (UnityEngine.Rendering.Universal.Light2D light in lightsToTurnOn)
            {
                light.enabled = true; // Toggle the state of the light
            }
        }
    }
}