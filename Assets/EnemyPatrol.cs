using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] float aimgTime = 0.5f;
    [SerializeField] float shootingTime = 1f;
    [SerializeField] float wallAware = 0.5f;
    [SerializeField] float speed = 1;
    [SerializeField] float minX;
    [SerializeField] float maX;
    [SerializeField] float waitingTime = 2f;
    [SerializeField] bool facingRight;
    [SerializeField] bool isAttacking;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject target;
    [SerializeField] Animator anim;
    [SerializeField] Weapon weapon;
    [SerializeField] Rigidbody2D rig;



    void Awake()
    {
        /*  StartCoroutine("PatrolToTarget");
          UpdateTarget();*/
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();

    }

    private void Start()
    {
        if (transform.localScale.x < 0f)
        {
            facingRight = false;
        }
        else if (transform.localScale.x > 0f)
        {
            facingRight = true;
        }
    }

    private void Update()
    {
        Vector2 direction = Vector2.right;

        if (!facingRight)
        {
            direction = Vector2.left;
        }
        if (!isAttacking)
        {
            if(Physics2D.Raycast(transform.position, direction, wallAware, groundLayer))
            {
                Flip();
            }
        }
    }


    void FixedUpdate()
    {
        float hVelocity = speed;

        if (!facingRight)
        {
            hVelocity *= -1;
        }

        if (isAttacking)
        {
            hVelocity = 0f;
        }

        rig.velocity = new Vector2(hVelocity, rig.velocity.y);
    }

    private void UpdateTarget()
    {

        if (target.transform.position.x == minX)
        {

            target.transform.position = new Vector2(maX, transform.position.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (target.transform.position.x == maX)
        {

            target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void AddDamage()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator PatrolToTarget()
    {
        while (Vector2.Distance(transform.position, target.transform.position) > 0.05f)
        {

            anim.SetBool("Idle", false);

            Vector2 direction = target.transform.position - transform.position;
            float xDirection = direction.x;
            transform.Translate(direction.normalized * speed * Time.deltaTime);

            yield return null;
        }

        anim.SetBool("Idle", true);

        anim.SetTrigger("Shoot");


        transform.position = new Vector2(target.transform.position.x, transform.position.y);

        UpdateTarget();

        yield return new WaitForSeconds(waitingTime);
            

        StartCoroutine("PatrolToTarget");

    }

    IEnumerator AimAndShoot()
    {
       
        isAttacking = true;
        yield return new WaitForSeconds(aimgTime);

        anim.SetTrigger("Shoot");

        anim.SetBool("Idle", true);

        yield return new WaitForSeconds(shootingTime);

        isAttacking = false;

        anim.SetBool("Idle", false);



    }

    public void CanShoot()
    {
        weapon.Shoot();
    }

    private void OnEnable()
    {
        isAttacking = false;
    }

    private void OnDisable()
    {
        StopCoroutine("AimAndShoot");
        isAttacking = false;
    }

    void Flip()
    {
        facingRight = !facingRight;

        float localScaleX = transform.localScale.x;
        localScaleX *= -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!isAttacking && collision.CompareTag("Player"))
        {
            anim.SetBool("Idle", true);

            StartCoroutine("AimAndShoot");
        }
    }

   

}
