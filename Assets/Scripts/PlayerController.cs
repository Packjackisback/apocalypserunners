using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    Rigidbody2D rb;
    Vector2 moveInput;

    public float maxHealth = 10f;
    float health;

    private Animator animator;

    private bool isShooting = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootRoutine());
        } else
        {
            handleMovement();
        }
    }

    System.Collections.IEnumerator ShootRoutine()
    {
        isShooting = true;
        animator.SetTrigger("isShooting");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isShooting = false;
    }
    void handleMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Math.Abs(moveInput.x) > 0.5)
        {
            animator.SetFloat("LastMoveX", moveInput.x);
            animator.SetFloat("LastMoveY", 0);
        }

        if (Math.Abs(moveInput.y) > 0.5)
        {
            animator.SetFloat("LastMoveY", moveInput.y);
            animator.SetFloat("LastMoveX", 0);
        }

        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);

        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void changeHealth(float impact)
    {
        if(health + impact < maxHealth)
        {
            health += impact;
        } else
        {
            health = maxHealth;
        }

        if(health < 0)
        {
            health = 0;
            StateManager.GameOver();
        }

        Debug.Log("Health is " + health);
    }

    public void dealDamage(float impact)
    {
        Debug.Log("Dealing " + impact + "damage");
        changeHealth(-impact);
    }

    public void healHealth(float impact)
    {
        Debug.Log("Healing " + impact + "health");
        changeHealth(impact);
    }
}
