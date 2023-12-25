using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GateController : MonoBehaviour
{

    // Reference to the Tilemap component
    public Tilemap gateTilemap;

    public AudioSource gateclose;

    private void Start()
    {
        // Ensure the gate is initially closed (inactive)
        CloseGate();
    }


    public void OpenGate()
    {
        // Activate the Tilemap to make the gate visible
        gateclose.Play();
        gateTilemap.gameObject.SetActive(true);
    }


    public void CloseGate()
    {
        gateTilemap.gameObject.SetActive(false);
    }
}