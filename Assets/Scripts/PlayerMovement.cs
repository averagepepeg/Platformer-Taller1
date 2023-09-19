using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float runSpeed = 2f;
    private float moveDirection = 0f;
    [SerializeField]private float jumpSpeed = 8f;
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D Capsulecollider;
    private float horizontal;
  
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;



    

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();    
        animator = GetComponent<Animator>();
        Capsulecollider = GetComponent<CapsuleCollider2D>();
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && moveDirection !=0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            animator.SetBool("WallContact",true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("WallContact",false);
        }
    }
    private void Grounded()
    {
        if(IsGrounded())
        {
            animator.SetBool("IsFalling",false);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<float>();
        
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
        WallSlide();
        Grounded();
        
    }

    
}
    
    
