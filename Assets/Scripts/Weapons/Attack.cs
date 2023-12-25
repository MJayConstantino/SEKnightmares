using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    public GameObject WeaponParentMelee, WeaponParentRanged;
    private InputActionReference attack, pointerPosition;
    

    // Update is called once per frame
    void Update()
    {
        if(attack.action.triggered)
        {

        }
    }

    void OnAttack()
    {

    }
}
