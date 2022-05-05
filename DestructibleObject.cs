using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public float health;
    public GameObject destroyEffect;

    private void Update()
    {
        DestroyObjects();
    }

    public void DestroyObjects()
    {
        if (health <= 0)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
