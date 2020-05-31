using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    [SerializeField] int damage;
    [SerializeField] bool returning;

    public Vector2 direction;

    [SerializeField] Rigidbody2D rig;

    [SerializeField] InstantiatePrefabs instantiate;

    void Awake()
    {
        instantiate = GetComponent<InstantiatePrefabs>();


        rig = GetComponent<Rigidbody2D>();

        Destroy(this.gameObject, 3f);
        
    }

    void FixedUpdate()
    {

        Vector2 movement = direction.normalized * speed;
        rig.velocity = movement;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!returning && collision.CompareTag("Player"))
        {
            collision.SendMessageUpwards("AddDamage", damage);
            Destroy(this.gameObject);

        }

        if(returning && collision.CompareTag("Enemy"))

        {
            Debug.Log(collision.name);

            collision.gameObject.transform.parent.GetComponent<EnemyPatrol>().AddDamage();
            collision.GetComponent<InstantiatePrefabs>().Instance();
            Destroy(this.gameObject);


        }


        if (returning && collision.CompareTag("Big Bullet"))

        {
            Debug.Log(collision.name);


            instantiate.Instance();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);


        }
    }

    public void AddDamage()
    {
        returning = true;
        direction *= -1f;
    }
}
