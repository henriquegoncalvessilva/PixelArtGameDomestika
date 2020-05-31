using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Transform firePoint;

    [SerializeField] GameObject shooter;

    [SerializeField] GameObject explosionEffect;

    [SerializeField] LineRenderer lineRenderer;

    private void Awake()
    {
        
        
    }

    void Start()
    {
        
    }

    public void Shoot()
    {

        Debug.LogWarning(shooter.name);

        GameObject mybullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity) as GameObject;

        Bullet bulletComponent = mybullet.GetComponent<Bullet>();

        if(shooter.transform.localScale.x < 0f)
        {

            //StartCoroutine(ShootWithRaycast());

            Debug.LogWarning(shooter.transform.localScale);

            bulletComponent.direction = Vector2.left;
        }
        else 
        {
           // StartCoroutine(ShootWithRaycast());

            Debug.LogWarning(shooter.transform.localScale);

            bulletComponent.direction = Vector2.right;
        }
    }

    public IEnumerator ShootWithRaycast()
    {
        if(explosionEffect != null && lineRenderer != null)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);


            if (hitInfo)
            {

                Instantiate(explosionEffect, hitInfo.point, Quaternion.identity);

                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, hitInfo.point);





            }

            else
            {
                lineRenderer.SetPosition(0, firePoint.position);
                lineRenderer.SetPosition(1, hitInfo.point + Vector2.right * 100);
            }
            
            lineRenderer.enabled = true;

            yield return null;

            lineRenderer.enabled = false;
           
        }
    }
}
