using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float jumpforca = 5f;
    [SerializeField] float speed = 3f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private bool attackbotao = false;
    private bool jumpbotao = false;
    private bool fireballbotao = false;
    private bool shootbotao = false;
    private bool dancebotao = false;
    private bool kickbotao = false;
    private bool runbotao = false;
    private bool laybotao = false;
    private float walkbotao = 0f;
    private bool isGrounded = false;

    enum State { Idle, Walk, Run, Jump1, Jump2, Attack, Fireball, Kick, ShootArrow, Dance, Lay }

    State state = State.Idle;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        attackbotao = Input.GetKey(KeyCode.E);
        jumpbotao = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
        fireballbotao = Input.GetKey(KeyCode.T);
        shootbotao = Input.GetKey(KeyCode.R);
        dancebotao = Input.GetKey(KeyCode.Q);
        kickbotao = Input.GetKey(KeyCode.F);
        laybotao = Input.GetKey(KeyCode.L);
        runbotao = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        walkbotao = Input.GetAxisRaw("Horizontal");

        switch (state)
        {
            case State.Idle: Idle(); break;
            case State.Walk: Walk(); break;
            case State.Run: Run(); break;
            case State.Jump1: Jump1(); break;
            case State.Jump2: Jump2(); break;
            case State.Attack: Attack(); break;
            case State.Fireball: Fireball(); break;
            case State.Kick: Kick(); break;
            case State.ShootArrow: ShootArrow(); break;
            case State.Dance: Dance(); break;
            case State.Lay: Lay(); break;
        }
    }
    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }

    private void CloseState(string animationName)
    {
        if (IsAnimationFinished(animationName))
        {
            rb.gravityScale = 1;

            if (isGrounded)
            {
                state = State.Idle;
            }
            else state = State.Jump2;
        }
    }
    private void Idle()
    {
        animator.Play("idle");
        if (attackbotao)
        {
            state = State.Attack;
        }
        else if (fireballbotao)
        {
            state = State.Fireball;
        }
        else if (laybotao)
        {
            state = State.Lay;
        }
        else if (walkbotao != 0)
        {
            state = State.Walk;
        }
        else if (kickbotao)
        {
            state = State.Kick;
        }
        else if (shootbotao)
        {
            state = State.ShootArrow;
        }
        else if (dancebotao)
        {
            state = State.Dance;
        }
        else if (jumpbotao && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0, jumpforca), ForceMode2D.Impulse);
            state = State.Jump1;
        }
        else if (!isGrounded)
        {
            state = State.Jump2;
        }
    }

    private void Walk()
    {
        animator.Play("walk");
        if (walkbotao < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbotao > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(walkbotao * speed, rb.velocity.y);
        if (runbotao)
        {
            state = State.Run;
        }
        if (jumpbotao && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0, jumpforca), ForceMode2D.Impulse);
            state = State.Jump1;
        }
        else CloseState("walk");
    }
    private void Run()
    {
        animator.Play("run");
        if (walkbotao < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbotao > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(walkbotao * 2 * speed, rb.velocity.y);
        if (jumpbotao && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0, jumpforca), ForceMode2D.Impulse);
            state = State.Jump1;
        }
        CloseState("run");
    }
    private void Jump1()
    {
        animator.Play("jump1");
        if (walkbotao < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbotao > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(walkbotao * speed, rb.velocity.y);

        if (rb.velocity.y < 0) state = State.Jump2;
        CloseState("jump1");
    }
    private void Jump2()
    {
        animator.Play("jump2");
        if (walkbotao < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (walkbotao > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        rb.velocity = new Vector2(walkbotao * speed, rb.velocity.y);
        CloseState("jump2");
    }
    private void Fireball()
    {
        animator.Play("castfireball");

        CloseState("castfireball");
    }
    private void Lay()
    {
        animator.Play("lay");

        CloseState("lay");
    }
    private void Attack()
    {
        animator.Play("attack");

        CloseState("attack");
    }
    private void Kick()
    {
        animator.Play("kick");

        CloseState("kick");
    }
    private void ShootArrow()
    {
        animator.Play("shootarrow");

        CloseState("shootarrow");
    }
    private void Dance()
    {
        animator.Play("dance");

        CloseState("dance");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chao"))
        {
            isGrounded = true;
        }

    }
}
