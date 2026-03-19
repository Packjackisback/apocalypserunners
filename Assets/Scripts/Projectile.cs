using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public AudioClip hitSound;

    // Add this line
    public ParticleSystem hitParticlesPrefab;

    private AudioSource audioSource;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (transform.position.magnitude > 1000f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
{
    if (other.gameObject.CompareTag("Zombie"))
    {
        Destroy(other.gameObject);

        if (hitParticlesPrefab != null)
        {
            ParticleSystem ps = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }

    Destroy(gameObject);
}
}