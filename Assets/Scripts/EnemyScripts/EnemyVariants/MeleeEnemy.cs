using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [Header("Melee Specific")]
    [SerializeField] private AudioSource walkSound;

    protected override void Start()
    {
        experienceValue = 1;
        moveSpeed = 2f;
        maxHealth = 10f;
        damageAmount = 1;
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