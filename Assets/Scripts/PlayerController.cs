using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public bool canMove = true;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public Animator animator;

    [Header("Health & Hunger")]
    public float maxHealth = 10f;
    public float maxHunger = 100f;
    private float health;
    private float hunger;
    private Coroutine hungerCoroutine;

    [Header("Combat")]
    public GameObject projectilePrefab;
    public float bulletSpeed = 50f;
    private bool isShooting = false;
    private bool isStabbing = false;

    [Header("Hunger Settings")]
    public float hungerDrainInterval = 1f;
    public float hungerDrainAmount = 0.5f;

    [Header("Interaction")]
    public float interactDistance = 2f;
    public LayerMask interactLayer;
    public GameObject interactUI;
    public Transform objectToMove;
    public float moveSpeedCutscene = 2f;
    private bool canInteract = false;
    private bool hasGun = false;

    [Header("NPC Interaction")]
    public GameObject npcTextUI;
    private bool isTalkingToNPC = false;
    public bool hasTalkedToNPC = false;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip stabSound;
    public AudioClip moveSound;
    private AudioSource audioSource;

    [Header("Particles")]
    public ParticleSystem shootParticlesPrefab;
    public ParticleSystem hitParticlesPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        health = maxHealth;
        hunger = maxHunger;

        // Disable movement by default, only enable in World
        canMove = false;
        if (SceneManager.GetActiveScene().name == "World") canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        // Combat
        if (hasGun && Input.GetKeyDown(KeyCode.E) && !isShooting && !isStabbing)
            Launch();
        else if (Input.GetMouseButtonDown(1) && !isShooting && !isStabbing)
            StartCoroutine(StabRoutine());
        else
            HandleMovement();

        animator.SetFloat("Velocity", Mathf.Max(Mathf.Abs(rb.linearVelocity.x), Mathf.Abs(rb.linearVelocity.y)));

        // Check interactions
        CheckForInteraction();

        // Handle interaction input
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(animator.GetFloat("LastMoveX"), animator.GetFloat("LastMoveY")), interactDistance, interactLayer);
            if (hit.collider != null)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                    interactable.Interact();
            }
        }
    }

    void HandleMovement()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(moveInput.x) > 0.5f)
        {
            animator.SetFloat("LastMoveX", moveInput.x);
            animator.SetFloat("LastMoveY", 0);
        }
        if (Mathf.Abs(moveInput.y) > 0.5f)
        {
            animator.SetFloat("LastMoveY", moveInput.y);
            animator.SetFloat("LastMoveX", 0);
        }

        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);

        if ((moveInput.x != 0 || moveInput.y != 0) && !audioSource.isPlaying)
            PlaySound(moveSound);

        moveInput = moveInput.normalized;
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Launch()
    {
        if (projectilePrefab == null) return;

        Vector2 lookDir = new Vector2(animator.GetFloat("LastMoveX"), animator.GetFloat("LastMoveY"));
        if (lookDir == Vector2.zero) lookDir = Vector2.right;
        lookDir.Normalize();

        Vector2 spawnPos = rb.position + lookDir *0.2f;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null) projectile.Launch(lookDir, bulletSpeed);

        if (shootParticlesPrefab != null)
        {
            ParticleSystem ps = Instantiate(shootParticlesPrefab, spawnPos, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        PlaySound(shootSound);
    }

    IEnumerator StabRoutine()
    {
        isStabbing = true;
        animator.SetFloat("Attacking", 2);
        PlaySound(stabSound);
        yield return new WaitForSeconds(0.3f);
        animator.SetFloat("Attacking", 0);
        isStabbing = false;
    }

    void CheckForInteraction()
    {
        Vector2 direction = new Vector2(animator.GetFloat("LastMoveX"), animator.GetFloat("LastMoveY"));
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
        }
        else
        {
            interactUI.SetActive(false);
            canInteract = false;
        }
    }

    IEnumerator EnterWorldRoutine()
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
                mainCam.transform.position = new Vector3(objectToMove.position.x, objectToMove.position.y, mainCam.transform.position.z);

            timer += Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("World");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GunPickup"))
        {
            PickupGun();
            Destroy(other.gameObject);
        }

        Food food = other.GetComponent<Food>();
        if (food != null) food.Consume(this);
    }

    public void PickupGun() => hasGun = true;

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null) audioSource.PlayOneShot(clip);
    }

    private void changeHealth(float impact)
    {
        health = Mathf.Min(maxHealth, health + impact);
        if (health <= 0) StateManager.GameOver();
        UIHealthBar.instance.SetValue(health / maxHealth);
    }

    public void dealDamage(float impact) => changeHealth(-impact);
    public void healHealth(float impact) => changeHealth(impact);

    private void changeHunger(float impact)
    {
        hunger = Mathf.Min(maxHunger, hunger + impact);
        if (hunger <= 0)
        {
            hunger = 0;
            dealDamage(0.2f);
            StateManager.GameOver();
        }
        HungerHealthBar.instance.SetValue(hunger / maxHunger);
    }

    public void Eat(float impact) => changeHunger(impact);
    public void LoseHunger(float impact) => changeHunger(-impact);

    public void StartHungerDrain()
    {
        if (hungerCoroutine != null) StopCoroutine(hungerCoroutine);
        hungerCoroutine = StartCoroutine(HungerDrainRoutine());
    }

    IEnumerator HungerDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDrainInterval);
            changeHunger(-hungerDrainAmount);
        }
    }

    void OnEnable()
    {
        GameEvent.OnTutorialStarted += DisableMovement;
        GameEvent.OnTutorialCompleted += EnableMovement;
    }

    void OnDisable()
    {
        if (hungerCoroutine != null) StopCoroutine(hungerCoroutine);
        GameEvent.OnTutorialStarted -= DisableMovement;
        GameEvent.OnTutorialCompleted -= EnableMovement;
    }

    void DisableMovement() => canMove = false;
    void EnableMovement() => canMove = true;
}