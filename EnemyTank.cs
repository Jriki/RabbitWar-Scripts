using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    private float timeBtwAttacks;
    public float startTimeBtwAttacks;
    public GameObject projectile;
    public Transform firePoint;
    public float minForce;
    public float maxForce;

    public Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwAttacks = startTimeBtwAttacks;
    }

    private void Update()
    {
        AttackControll();
    }

    private void AttackControll()
    {
        if (timeBtwAttacks <= 0)
        {
            Vector2 leftDir = Quaternion.AngleAxis(-90, transform.up) * transform.forward;

            GameObject newProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = leftDir * Random.Range(minForce, maxForce);
            FindObjectOfType<AudioManager>().PlaySound("EnemyFireSound");

            timeBtwAttacks = startTimeBtwAttacks;
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

    }

}
