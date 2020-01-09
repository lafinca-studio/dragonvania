using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // config
    [SerializeField] float speed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(5f, 5f);
    [SerializeField] float climbSpeed = 5f;

    // state
    bool isAlive = true;
    bool isFacingRight;

    // cache component references
    Animator animator;
    Rigidbody2D rigidbody;
    BoxCollider2D heroFeet;
    CapsuleCollider2D body;
    float gravity;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        heroFeet = GetComponent<BoxCollider2D>();
        body = GetComponent<CapsuleCollider2D>();
        gravity = rigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        run();
        flipSprite();
        jump();
        die();
        climb();
    }

    private void climb()
    {
        if (!body.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            animator.SetBool("Climbing", false);
            rigidbody.gravityScale = gravity;
            return; 
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(rigidbody.velocity.x, controlThrow * climbSpeed);
        rigidbody.velocity = climbVelocity;
        rigidbody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(rigidbody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("Climbing", playerHasVerticalSpeed);
        animator.SetBool("Jumping", false);
        animator.SetBool("Running", false);
    }

    private void run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");

        bool playerHasHorizontalMove = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        bool playerJumpVertical = Mathf.Abs(rigidbody.velocity.y) > Mathf.Epsilon;

        Vector2 playerVelocity = new Vector2(controlThrow * speed, rigidbody.velocity.y);
        rigidbody.velocity = playerVelocity;

        if (!playerJumpVertical)
        {
            animator.SetBool("Running", playerHasHorizontalMove);
        }
    }

    private void jump()
    {
        bool playerClimbing = body.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        if (playerClimbing) {
            animator.SetBool("Jumping", false);
            return; 
        }
        bool playerJumpVertical = Mathf.Abs(rigidbody.velocity.y) > Mathf.Epsilon;
        bool playerOnGround = heroFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (CrossPlatformInputManager.GetButtonDown("Jump") && playerOnGround)
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            rigidbody.velocity = rigidbody.velocity + jumpVelocityToAdd;
            animator.SetBool("Running", false);
        }
        animator.SetBool("Jumping", playerJumpVertical);
    }

    private void flipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.velocity.x), 1f);
        }
    }

    private void die()
    {
        if (body.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Die");
            GetComponent<Rigidbody2D>().velocity = deathKick;

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
