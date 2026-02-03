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

    void Update()
    {
        if (target != null)
        {
            float step = speed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }
}
