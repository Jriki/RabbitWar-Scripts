using UnityEngine;

public class WinBorder : MonoBehaviour
{
    public GameManager gameManager;

    private void OnCollisionEnter2D(Collision2D other)
    {
        FindObjectOfType<AudioManager>().PlaySound("EnemyDeath");

        Destroy(gameObject);
        new WaitForSeconds(2);
        gameManager.CompleteLevel();
    }
}
