using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] bool isAttacking;

    [SerializeField] Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {

            isAttacking = true;

        }

        else

        {
            isAttacking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking)
        {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Big Bullet"))
            {
                Debug.Log(collision.name);

                collision.SendMessageUpwards("AddDamage");
            }
        }
    }

}
