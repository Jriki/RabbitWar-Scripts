using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 450f;                          
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  
	[SerializeField] private bool m_AirControl = false;                         
	[SerializeField] private LayerMask m_WhatIsGround;                          
	[SerializeField] private Transform m_GroundCheck;                           
	[SerializeField] private Transform m_CeilingCheck;                          

	const float k_GroundedRadius = .2f; 
	private bool m_Grounded;            
	const float k_CeilingRadius = .2f;

	private Rigidbody2D rb;
	private Animator anim;
	private Vector3 m_Velocity = Vector3.zero;

	[Space]
	public float maxHealth = 500;
	public float currentHealth;
	public HealthBar healthBar;

	[Space]
	public GameObject deathEffect;
	public GameObject explosion;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		//anim = GetComponent<Animator>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}
    private void Start()
    {
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
		Die();
    }

    private void FixedUpdate()
	{
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

    #region Movement Method
    public void Move(float move, bool jump)
	{

		if (m_Grounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
		}
		// If the player should jump
		if (m_Grounded && jump)
		{
			m_Grounded = false;
			rb.AddForce(new Vector2(0f, m_JumpForce));
			FindObjectOfType<AudioManager>().PlaySound("Jump");
		}
	}

	#endregion

	public void PlayerTakeDamage(float damage)
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
			FindObjectOfType<AudioManager>().PlaySound("PlayerDeath");

			Destroy(gameObject, 1f);
			new WaitForSeconds(3);
			FindObjectOfType <GameManager>().EndGame();
		}
	}
}