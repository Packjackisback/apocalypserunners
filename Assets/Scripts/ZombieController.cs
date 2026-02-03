using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float speed = 6f;
    Rigidbody2D rb;
    public Transform target;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("Collision detected with " + collisionInfo.gameObject.name);

        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            PlayerController = collisionInfo.gameObject.GetComponent<PlayerController>();

            if(PlayerController != null)
        }
    }
}