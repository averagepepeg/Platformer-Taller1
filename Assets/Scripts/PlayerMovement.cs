using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{

    private float moveDirection = 0f;
    private float dir = 1f;
    private bool CanDoubleJump;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCD = 1f;


    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D Capsulecollider;
    //private bool isWallSliding;

    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float DoblejumpSpeed = 10f;
    [SerializeField] private float wallSlidingSpeed = 0.5f;
    [SerializeField] private TrailRenderer tr;



    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();    
        animator = GetComponent<Animator>();
        Capsulecollider = GetComponent<CapsuleCollider2D>();
    }

    private bool IsWalled()
    {
        
        // return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir, 0f),0.4f);
        if (hit)
        {
            //Debug.Log("IsWalled");
            CanDoubleJump = true;
            return true;  

        }
        else
        {
            return false;
        }
        
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded())
        {
            //isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x,Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            animator.SetBool("WallContact",true);
 
        }
        else
        {
            //isWallSliding = false;
            animator.SetBool("WallContact",false);
        }
    }
    private void Grounded()
    {
        if(IsGrounded())
        {
            animator.SetBool("IsFalling",false);
            animator.SetBool("WallContact",false);
        }
    }
    private bool IsGrounded()
    {
        //return Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0f, -1f),0.52f);
        if (hit)
        {
            //Debug.Log("IsGrounded");
            animator.SetBool("IsFalling",false);
            CanDoubleJump = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<float>();
        if (moveDirection ==1)
        {
            dir = 1f;
        }
        else if (moveDirection ==-1)
        {
            dir =-1f;
        }
        
    }
    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if(IsGrounded())
            //if(Capsulecollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                Debug.Log("GroundJump");
                animator.SetBool("IsJumping",true);
                rb.velocity += new Vector2(0f, jumpSpeed);
            }
            if(IsWalled())
            {
                //rb.velocity += new Vector2(0f, jumpSpeed);
               rb.velocity += new Vector2(-20f,8f);
               Debug.Log("Forsan");
               animator.SetBool("IsJumping",true);
            }
            if (!IsGrounded() && !IsWalled() && CanDoubleJump)
            {
                Debug.Log("DobleJump");
                rb.velocity += new Vector2(0f, DoblejumpSpeed);
                CanDoubleJump = false;
            }

        }
      
    }
   /* private void OnDesh(InputValue value)
    {
        if (value.isPressed && canDash)
        {
            StartCoroutine(Dash());
        }
           
    }*/
    private void Airborne()
    {
        if (Mathf.Sign(rb.velocity.y) < 0 && animator.GetBool("IsJumping")== true) 
        {
            animator.SetBool("IsJumping",false);
            animator.SetBool("IsFalling",true);
            rb.gravityScale = 2f;
        }
        
    }
    private void Run()
    {
        if (moveDirection == 0f)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        };


        rb.velocity = new Vector2(runSpeed * moveDirection, rb.velocity.y);
        
    }
    private void FlipSprite()
    {
        if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        transform.localScale = new Vector3 (Mathf.Sign(rb.velocity.x),1f,1f);
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dir * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCD);
        canDash = true; ;
    }
    // private void OnCollisionEnter2D(Collision2D other) 
    // {
    //     if (other.transform.CompareTag("Platform"))
    //     {
    //        animator.SetBool("IsFalling",false);
    //         animator.SetBool("IsJumping",false);
    //         rb.gravityScale = 1f;
    //     }
           
    // }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        Run();
        FlipSprite();
        Airborne();
        //IsWalled();
        //IsGrounded();
        WallSlide();
        Grounded();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        Debug.DrawRay(transform.position,new Vector2(dir,0)*10f, Color.red);
        Debug.DrawRay(transform.position,new Vector2(0f,-1f)*0.50f, Color.blue);
    }

    
}
    
    
