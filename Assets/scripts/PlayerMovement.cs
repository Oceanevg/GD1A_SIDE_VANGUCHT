using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float moveSpeed;
	public float jumpForce;
    public float climbSpeed;

    public bool isJumping;
	public bool isGrounded;
    public bool isClimbing;

    public Transform groundCheckLeft;
	public Transform groundCheckRight;

	public Rigidbody2D rb;
	public Animator animator;
	public Animator healthBar;
	public SpriteRenderer spriteRenderer;

    private float verticalMovement;
    private float horizontalMovement;

    public int PointDeVie = 2;

	private Vector3 velocity = Vector3.zero;


	void Update()
	{
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
		{
			isJumping = true;
		}
	}

	void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);
		float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

		

		MovePlayer(horizontalMovement, verticalMovement);

		Flip(rb.velocity.x);
		float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
	}

	void MovePlayer(float _horizontalMovement, float _verticalMovement)
	{
        if (!isClimbing)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

            if (isJumping)
            {
                rb.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
            }
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0, _verticalMovement);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        }
    }



	void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.layer == 6)
		{
			PointDeVie -= 1;
			if (PointDeVie == 1)
			{
				healthBar.Play("UnCoeur");
			}
			else if ( PointDeVie <= 0) 
			{
				healthBar.Play("ZeroCoeur");
			}
		}

        if (collision.gameObject.layer == 7)
        {
            PointDeVie -= 2;
			healthBar.Play("ZeroCoeur");
    
        }

    }
}