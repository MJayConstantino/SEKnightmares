using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GateController : MonoBehaviour
{
    public Tilemap gateTilemap;

    public AudioSource gateclose;

    private void Start()
    {
        CloseGate();
    }

    public void OpenGate()
    {
        gateclose.Play();
        gateTilemap.gameObject.SetActive(true);
    }

    public void CloseGate()
    {
        gateTilemap.gameObject.SetActive(false);
    }
}