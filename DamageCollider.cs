using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
            FindObjectOfType<AudioManager>().PlaySound("PlayerDeath");

            Destroy(gameObject, 1f);

            FindObjectOfType<GameManager>().EndGame();
    }
}
