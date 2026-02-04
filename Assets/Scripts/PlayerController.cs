using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    Rigidbody2D rb;
    Vector2 moveInput;

    public float maxHealth = 10f;
    float health;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
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
