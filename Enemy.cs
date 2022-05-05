using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;

    public float speed;
    public float minDistance;
    public float retreatDistance;

    public Transform player;

    [Space]
    public float maxHealth = 800;
    public float currentHealth;
    public HealthBar healthBar;

    public GameObject deathEffect;
    public GameObject explosion;
    public Animator animator;


    private Rigidbody2D rb;

    [SerializeField] private LayerMask m_WhatIsGround;                          
    [SerializeField] private Transform m_GroundCheck;                           
    [SerializeField] private Transform m_CeilingCheck;                         

    const float k_GroundedRadius = .2f; 
    private bool m_Grounded;            
    const float k_CeilingRadius = .2f;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Die();
    }

    private void FixedUpdate()
    {
        EnemyMovement();

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    private void EnemyMovement()
    {
        if (player == true)
        {
            if (Vector2.Distance(transform.position, player.position) > minDistance)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < minDistance &&
                Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }
        }
        else
        {
            player = null;
        }


    }

    public void TakeDamage(float damage)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void Die()
    {
        if (currentHealth < 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().PlaySound("EnemyDeath");

            Destroy(gameObject);
            new WaitForSeconds(2);
            gameManager.CompleteLevel();
        }
    }

}
