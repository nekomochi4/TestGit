using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float JumpForce = 15f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Player Movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, rb.velocity.y);

        // Sprite Flip
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // ”÷¬‚È‘¬“x‚Í–³Ž‹
        {
            if (rb.velocity.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (rb.velocity.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

}