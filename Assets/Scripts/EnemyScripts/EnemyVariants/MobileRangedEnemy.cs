using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileRangedEnemy : RangedEnemy
{
    [Header("Mobile Enemy Specific")]
    [SerializeField] private AudioSource dragonFly;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (canMove && target)
        {
            dragonFly?.Play();
        }
    }
}
