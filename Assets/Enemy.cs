using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D rigidbody;
    Animator animator;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        if (isFacingRight())
        {
            rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-moveSpeed, rigidbody.velocity.y);
        }
    }

    private bool isFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rigidbody.velocity.x)), 1f);
    }
}
