using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float speed = 6f;
    Rigidbody2D rb;
    public Transform target;
    public float damage = 0.8f;
    public float health = 3f;
    public bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    void Start()
{
    if (target == null)
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }
}
    void Update()
    {
        if (!canMove)
            return;
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;

            Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
        Debug.Log("Going");
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Debug.Log("Collision detected with " + collisionInfo.gameObject.name);

        if (collisionInfo.gameObject.CompareTag("Player"))
        {
            PlayerController pc = collisionInfo.gameObject.GetComponent<PlayerController>();

            if(pc != null) {
                pc.dealDamage(damage);
            }
        }
    }

    private void changeHealth(float impact)
    {
        if (health < 0)
        {
            die();
        }
    }

    public void die()
    {
        //TODO ANIMATE DEATH
        Destroy(gameObject);
    }

    public void dealDamage(float impact)
    {
        changeHealth(-impact);
    }

    public void healHealth(float impact)
    {
        changeHealth(impact);
    }
    void OnEnable()
    {
        GameEvent.OnTutorialStarted += DisableMovement;
        GameEvent.OnTutorialCompleted += EnableMovement;
    }

    void OnDisable()
    {
        GameEvent.OnTutorialStarted -= DisableMovement;
        GameEvent.OnTutorialCompleted -= EnableMovement;
    }

    void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

}