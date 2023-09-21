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
    [SerializeField]private float runSpeed = 4f;
    private float moveDirection = 0f;
    private float dir = 1f;
    [SerializeField]private float jumpSpeed = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D Capsulecollider;
    //private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 0.5f;


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
            animator.SetBool("IsFalling",false);
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
            if(Capsulecollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                animator.SetBool("IsJumping",true);
                rb.velocity += new Vector2(0f, jumpSpeed);
            }
            else if(IsWalled())
            {
                            Debug.Log("Forsan");
                
                //rb.velocity += new Vector2(0f, jumpSpeed);
               // rb.velocity += new Vector2(-100f,8f);
               rb.AddForce(new Vector2(-100f,8f));
                animator.SetBool("IsJumping",true);
            }
            
        }
      
    }
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
        Run();
        FlipSprite();
        Airborne();
        IsWalled();
        //IsGrounded();
        WallSlide();
        Grounded();
        Debug.DrawRay(transform.position,new Vector2(dir,0)*10f, Color.red);
        Debug.DrawRay(transform.position,new Vector2(0f,-1f)*0.50f, Color.blue);
    }

    
}
    
    
