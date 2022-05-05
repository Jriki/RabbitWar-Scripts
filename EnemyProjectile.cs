using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    Rigidbody2D rb;
    bool hasHit;

    public float lifeTime;
    public float damage;

    public float areaOfEffect;
    public LayerMask whatIsDestructible;

    public GameObject destroyEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyProjectile", lifeTime);
    }

    private void FixedUpdate()
    {
        ProjectileControll();
    }

    private void ProjectileControll()
    {

        if (hasHit == false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(damage);
            FindObjectOfType<AudioManager>().PlaySound("EnemyExplosion");
        }
        else if (collision.transform.CompareTag("Destructible"))
        {
            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, areaOfEffect, whatIsDestructible);
            for (int i = 0; i < objectsToDamage.Length; i++)
            {
                objectsToDamage[i].GetComponent<DestructibleObject>().health -= damage;
                FindObjectOfType<AudioManager>().PlaySound("EnemyExplosion");
            }
        }
        DestroyProjectile();
    }


    void DestroyProjectile()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}
