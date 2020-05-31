using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRayCast2D : MonoBehaviour
{

    [SerializeField] Animator anim;
    [SerializeField] Weapon weapon;

    void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();

        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetBool("Idle", true);

    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Shoot");
        }
    }

    void CanShoot()
    {
        StartCoroutine(weapon.ShootWithRaycast());
    }
}
