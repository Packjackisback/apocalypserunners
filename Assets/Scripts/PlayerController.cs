using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
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
    private bool isStabbing = false;
    public bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        animator = GetComponent<Animator>();
        canMove = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "World") canMove = true;
        if (!canMove)
            return;
        if (Input.GetMouseButtonDown(0) && !isShooting && !isStabbing)
        {
            StartCoroutine(ShootRoutine());
        }
        else if (Input.GetMouseButtonDown(1) && !isShooting && !isShooting)
        {
            StartCoroutine(StabRoutine());
        }
        else
        {
            handleMovement();
        }

        animator.SetFloat("Velocity", Math.Max(Math.Abs(rb.linearVelocityX), Math.Abs(rb.linearVelocityY)));
    }

    System.Collections.IEnumerator ShootRoutine()
    {
        isShooting = true;
        animator.SetFloat("Attacking", 1);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetFloat("Attacking", 0);
        isShooting = false;
    }

    System.Collections.IEnumerator StabRoutine()
    {
        isStabbing = true;
        animator.SetFloat("Attacking", 2);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetFloat("Attacking", 0);
        isStabbing = false;
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
        if (health + impact < maxHealth)
        {
            health += impact;
        }
        else
        {
            health = maxHealth;
        }

        if (health < 0)
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
    void OnEnable()
    {
        GameEvents.OnTutorialStarted += DisableMovement;
        GameEvents.OnTutorialCompleted += EnableMovement;
    }

    void OnDisable()
    {
        GameEvents.OnTutorialStarted -= DisableMovement;
        GameEvents.OnTutorialCompleted -= EnableMovement;
    }

    void DisableMovement()
    {
        canMove = false;
    }

    void EnableMovement()
    {
        canMove = true;
    }

}
