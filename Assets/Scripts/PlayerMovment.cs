using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    // Components
    public Rigidbody2D rb;
    Animator animator;
    public AudioSource walkSound;
    public DeathAndHurtManager deadScript;
    public Light light;

    // Walk
    public float maxWalkSpeed;
    float walkSpeed=0;
    public float walkSpeedIncrease;
    float movment; //AxisInput x
    bool facingRight = true; //Player Direction
    float movmentSpeedTemp;
    public float flippingTime = 0.1f; // Time in which the character flips

    // Jump
    public float jumpSpeed;
    bool canJump = true;
    public float checkRadius;
    public LayerMask whatIsGround;   
    float gravityScale; 
    public Transform groundCheck; // Reference point from which the ground is checked
    int jumps = 2; // Number of remainging jumps (double jump in this case)
    bool relased = true;
    float jumpSpeedTemp;
    public float secondJumpMultiplier = 2f; 

    // Attack, Hurt and Death
    public bool canAttack = true;
    public bool isAttacked;
    public float waitTime;

    // Others
    public float effectTime;


    void Start()
    {
       rb = GetComponent<Rigidbody2D>(); 
       animator = GetComponent<Animator>();
       deadScript = FindObjectOfType<DeathAndHurtManager>();

       gravityScale = rb.gravityScale;
       jumpSpeedTemp = jumpSpeed;
       movmentSpeedTemp = maxWalkSpeed;
    }

    
    void FixedUpdate()
    {
        /// - - - - -  ATTACKING  - - - - - ///
        // If neighter of the attack animations are triggered is not atacking
        bool isAttacking = !animator.GetBool("Attack") && !animator.GetBool("Attack2");
        // If the left mouse is pressed and the character can attack
        if (Input.GetMouseButtonDown(0) && canAttack && isAttacking)
        {
            int a = Random.Range(1,3);
            if (a == 1)
            {
                animator.SetBool("Attack", true);
            }else{
                animator.SetBool("Attack2", true);
            }
            StartCoroutine("Attack_Stop");
            
            canAttack = false;
            // Set canAttack
            StartCoroutine("Set_Can_Attack_True");
        }
        ///  - - - - -  ///



        // - - - - -  MOVING HORIZONTALY  - - - - - ///    
        // Read the horizontal axis value
        movment = Input.GetAxis("Horizontal");

        // If the character is not atacking set the speed in order to trigger the animation
        if (animator.GetBool("Attack") == false && animator.GetBool("Attack2") == false)
        {
            animator.SetFloat("Speed", Mathf.Abs(movment));
        }
        
        if (movment != 0f)
        { // movment loop
            if (maxWalkSpeed > walkSpeed)
            {
                walkSpeed += walkSpeedIncrease;
            }

            if (!walkSound.isPlaying)
            {
                walkSound.Play(0);
            }  
        }else{
            walkSpeed = 0;
            
            if (walkSound.isPlaying)
            {
                walkSound.Stop();
            }
            
        }
        // Move in the direction of the horizontal axis value at 'maxWalkSpeed'
        rb.velocity = new Vector2(walkSpeed * movment, rb.velocity.y);

        // Flip the character in the direction of the movment
        if ((movment > 0 && !facingRight) || (movment < 0 && facingRight))
        {
            StartCoroutine("Flip");
        }
        ///  - - - - -  ///



        /// - - - - -  JUMPING  - - - - - ///
        canJump = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (canJump)
        {
            jumps = 2;    
        }
        else
        {
            walkSound.Stop();
        }

        if ((Input.GetButton("Jump")) && (jumps > 0) && (relased))
        { 
            jumps --;
            relased = false;
            if (jumps == 2)
            {
                rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed);
            }
            else
            {
                rb.velocity = new Vector2 (rb.velocity.x, jumpSpeed * secondJumpMultiplier);
            }
            
        }

        if (!Input.GetButton("Jump"))
        {
            relased = true;
        }
        ///  - - - - -  ///


        
        /// - - - - -  SENDING THE DATA TO THE ANIMATOR  - - - - - ///
        if (animator.GetBool("Attack") == false && animator.GetBool("Attack2") == false)
        {
            animator.SetBool("Jump", Input.GetButton("Jump"));
            animator.SetFloat("JumpSpeed", rb.velocity.y);
            animator.SetBool("canJump", canJump);
        }
        ///  - - - - -  ///
    }

    
    public IEnumerator Flip()
    {    
        facingRight = !facingRight;
        yield return new WaitForSeconds(flippingTime);
        transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
    }


    public IEnumerator Attack_Stop()
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("Attack", false);
        animator.SetBool("Attack2", false);
    }


    public IEnumerator Set_Can_Attack_True()
    {
        yield return new WaitForSeconds(waitTime);
        canAttack = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "BlueGem")
        {
            jumpSpeed *= 1.5f;
            light.color = new Color(0f, 0.04f , 1.00f, 1f);
            StartCoroutine("Set_Jump_Speed_Normal");
        }
    }


    public IEnumerator Set_Jump_Speed_Normal()
    {
        yield return new WaitForSeconds(effectTime);
        jumpSpeed = jumpSpeedTemp;
        light.color = new Color(0.75f, 0.49f , 0.20f, 1f);
    }


    public bool getFacingRight()
    {
        return facingRight ? true : false;
    }
}
