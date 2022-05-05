using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public PlayerController controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

	void Update()
	{
		//arrow keys or a , d.
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		// default space button
		if (Input.GetButtonDown("Jump")) 
		{
			jump = true; 
		}

	}

	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}