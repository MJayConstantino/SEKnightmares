using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [Header("Melee Specific")]
    [SerializeField] private AudioSource walkSound;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (canMove && target)
        {
            walkSound?.Play();
        }
    }
}