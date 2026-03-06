using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    Rigidbody2D rb;
    Vector2 moveInput;

    public float maxHealth = 10f;
    float health;

    public float maxHunger = 100f;
    float hunger;

    private Animator animator;
    public GameObject projectilePrefab;
    private bool isShooting = false;
    private bool isStabbing = false;
    public bool canMove = true;
    public float hungerDrainInterval = 0.1f;
    public float hungerDrainAmount = 100f;
    private Coroutine hungerCoroutine;

    public float interactDistance = 2f;
    public LayerMask interactLayer;
    public GameObject interactUI;
    public Transform objectToMove;
    public float moveSpeedCutscene = 2f;

    private bool canInteract = false;

    // AUDIO
    public AudioClip shootSound;
    public AudioClip stabSound;
    public AudioClip moveSound;
    private AudioSource audioSource;
    public float bulletSpeed = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;
        hunger = maxHunger;
        animator = GetComponent<Animator>();

        canMove = false;
        if (SceneManager.GetActiveScene().name == "World") canMove = true;

        hungerCoroutine = StartCoroutine(HungerDrainRoutine());
    }

    void Update()
    {
        if (!canMove)
            return;

        if(Input.GetKeyDown(KeyCode.E) && !isShooting && !isStabbing)
        {
            Launch();
        }
        else if(Input.GetMouseButtonDown(1) && !isShooting && !isStabbing)
        {
            StartCoroutine(StabRoutine());
        }
        else
        {
            handleMovement();
        }

        animator.SetFloat("Velocity", Math.Max(Math.Abs(rb.linearVelocityX), Math.Abs(rb.linearVelocityY)));

        CheckForInteraction();

        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(EnterWorldRoutine());
        }
    }

   void Launch()

    {
        GameObject projectilePrefab = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(lookDirection, 300);
    }

    System.Collections.IEnumerator StabRoutine()
    {
        isStabbing = true;
        animator.SetFloat("Attacking", 2);

        PlaySound(stabSound);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetFloat("Attacking", 0);
        isStabbing = false;
    }

    System.Collections.IEnumerator HungerDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDrainInterval);
            changeHunger(-hungerDrainAmount);
        }
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
        if(moveInput.x>0||moveInput.y>0)
            PlaySound(moveSound);
        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void changeHealth(float impact)
    {
        if(health + impact < maxHealth)
        {
            health += impact;
        }
        else
        {
            health = maxHealth;
        }

        if(health <= 0)
        {
            health = 0;
            StateManager.GameOver();
        }

        UIHealthBar.instance.SetValue(health / maxHealth);
    }

    public void dealDamage(float impact)
    {
        Debug.Log("Dealing " + impact + " damage");
        changeHealth(-impact);
    }

    public void healHealth(float impact)
    {
        Debug.Log("Healing " + impact + " health");
        changeHealth(impact);
    }

    private void changeHunger(float impact)
    {
        if (hunger + impact < maxHunger)
        {
            hunger += impact;
        }
        else
        {
            hunger = maxHunger;
        }

        if (hunger <= 0)
        {
            hunger = 0;
            dealDamage(0.2f);
            StateManager.GameOver();
        }

        HungerHealthBar.instance.SetValue(hunger / maxHunger);
    }

    public void Eat(float impact)
    {
        Debug.Log("Eating " + impact + " food");
        changeHealth(impact);
    }

    public void LoseHunger(float impact)
    {
        Debug.Log("Draining " + impact + " food");
        changeHealth(-impact);
    }

    void OnEnable()
    {
        GameEvent.OnTutorialStarted += DisableMovement;
        GameEvent.OnTutorialCompleted += EnableMovement;
    }

    void OnDisable()
    {
        if (hungerCoroutine != null)
        {
            StopCoroutine(hungerCoroutine);
            hungerCoroutine = null;
        }

        GameEvent.OnTutorialStarted -= DisableMovement;
        GameEvent.OnTutorialCompleted -= EnableMovement;
    }

    void DisableMovement()
    {
        canMove = false;
    }

    void EnableMovement()
    {
        canMove = true;
    }

    void CheckForInteraction()
    {
        Vector2 direction = new Vector2(
            animator.GetFloat("LastMoveX"),
            animator.GetFloat("LastMoveY")
        );

        if (direction == Vector2.zero)
        {
            interactUI.SetActive(false);
            canInteract = false;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, interactDistance, interactLayer);

        if (hit.collider != null)
        {
            interactUI.SetActive(true);
            canInteract = true;
            Debug.Log("Raycast Hit");
        }
        else
        {
            interactUI.SetActive(false);
            canInteract = false;
        }
    }

    System.Collections.IEnumerator EnterWorldRoutine()
    {
        canMove = false;
        interactUI.SetActive(false);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        Camera mainCam = Camera.main;
        float timer = 0f;

        while (timer < 3.5f)
        {
            objectToMove.position += Vector3.right * moveSpeedCutscene * Time.deltaTime;

            if (mainCam != null)
            {
                mainCam.transform.position = new Vector3(
                    objectToMove.position.x,
                    objectToMove.position.y,
                    mainCam.transform.position.z
                );
            }

            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("World");
    }
}