using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateSnapshot
{
    public Vector2 Position;
    public int Health;
    public float TimeStamp;

    public PlayerStateSnapshot(Vector2 position, int health, float timeStamp)
    {
        Position = position;
        Health = health;
        TimeStamp = timeStamp;
    }
}