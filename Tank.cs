using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject tankProjectile;
    public GameObject fireEffect;

    private float timeBtwAttacks;
    public float startTimeBtwAttacks;

    public float minForce;
    public float maxForce;
    public float fireForce;
    public Transform firePoint;

    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBtwPoints;
    Vector2 direction;

    public CameraShake cameraShake;

    private void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, firePoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        TankControll();
    }

    public void TankControll()
    {
        Vector2 tankPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - tankPos;
        transform.right = direction;

        if (timeBtwAttacks <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                StartCoroutine(cameraShake.Shake(0.10f, 0.3f));

                GameObject newFireEffect = Instantiate(fireEffect, firePoint.position, Quaternion.identity);
                Destroy(newFireEffect, 1);
                timeBtwAttacks = startTimeBtwAttacks;
            }
        }
        else
        {
            timeBtwAttacks -= Time.deltaTime;
        }

        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].transform.position = PointsPos(i * spaceBtwPoints);
        }

    }


    void Shoot()
    {
        GameObject newProjectile = Instantiate(tankProjectile, firePoint.position, firePoint.rotation);
        newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * Random.Range(minForce, maxForce);
        FindObjectOfType<AudioManager>().PlaySound("FireSound");
    }

    Vector2 PointsPos(float t)
    {
        Vector2 position = (Vector2)firePoint.position + (direction.normalized * fireForce * t) + 0.5f * Physics2D.gravity * (t * t);
        return position;
    }

}
