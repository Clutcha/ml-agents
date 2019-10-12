using System;
using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class NinjaAgent : Agent
{
    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private float jumpForce;
    public float jumpTime;
    private float jumpTimeCounter;
    private bool isJumping;
    private bool isFirstJump;

    [Header("Ninja Agent Settings")]
    public float moveSpeed = 1f;

    private Rigidbody2D agentRigidbody;
    private Animator animator;

    bool isFacingRight;
    float direction;
    bool isGrounded;

    private void Start()
    {
        agentRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public override void AgentReset()
    {
        isFacingRight = true;
        isJumping = false;
        direction = 0;
        isFirstJump = true;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //Debug.Log(vectorAction[0].ToString() + " " + vectorAction[1].ToString());
        // Handle Movment
        if (vectorAction[0] == 0f)
        {
            direction = 0;
        }
        if (vectorAction[0] == 1f)
        {
            direction = 1;
            AddReward(0.1f);
        }
        else if (vectorAction[0] == 2f)
        {
            direction = -1;
            AddReward(0.1f);
        }

        Vector2 velocity;
        velocity = new Vector2(direction * moveSpeed, agentRigidbody.velocity.y);

        isGrounded = IsGrounded();
        if (vectorAction[1] == 1f && isGrounded)
        {
            isJumping = true;
            //jumpTimeCounter = jumpTime; 
            velocity.y = jumpForce;
        }
        //else if (vectorAction[1] == 1f && isJumping)
        //{
        //    if (jumpTimeCounter > 0)
        //    {
        //        velocity.y = jumpForce;
        //        jumpTimeCounter -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        isJumping = false; 
        //    }
        //}
        else if (vectorAction[1] != 1f)
        {
            isJumping = false;
        }

        agentRigidbody.velocity = velocity;
        animator.SetFloat("speed", Math.Abs(direction));
        Flip(direction);

        // Tiny negative reward
        AddReward(-1f / agentParameters.maxStep);
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private bool IsGrounded()
    {
        foreach (Transform point in groundPoints)
        {
            bool hasCollision = Physics2D.OverlapCircle(point.position, groundRadius, groundMask);
            if (hasCollision)
            {
                return true;
            }
        }
        
        return false;
    }

    public override void CollectObservations()
    {
        AddVectorObs(direction);
    }
}
