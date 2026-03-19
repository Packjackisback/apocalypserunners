using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialCar : MonoBehaviour, IInteractable
{
    public float moveSpeed = 2f;
    public bool IsTutorialInteraction => true;

    public void Interact()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            // Start the car movement coroutine
            player.StartCoroutine(MoveCarRoutine(player));
        }
    }

    private IEnumerator MoveCarRoutine(PlayerController player)
    {
        // Disable movement during the car cutscene
        player.canMove = false;

        // Keep the sprite visible during animation so you see the player moving
        SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
        Collider2D collider = player.GetComponent<Collider2D>();

        if (sprite != null) sprite.enabled = true;
        if (collider != null) collider.enabled = false;

        // Set animator for moving right
        if (player.animator != null)
        {
            player.animator.SetFloat("Horizontal", 1f);
            player.animator.SetFloat("Vertical", 0f);
            player.animator.SetFloat("LastMoveX", 1f);
            player.animator.SetFloat("LastMoveY", 0f);
        }

        Camera mainCam = Camera.main;
        float timer = 0f;

        while (timer < 3.5f)
        {
            // Move the car (not the player)
            player.GetComponent<SpriteRenderer>().enabled = false;
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            // Move camera to follow
            if (mainCam != null)
                mainCam.transform.position = new Vector3(
                    player.transform.position.x,
                    player.transform.position.y,
                    mainCam.transform.position.z
                );

            timer += Time.deltaTime;
            yield return null;
        }

        // Hide player only after the cutscene finishes
        if (sprite != null) sprite.enabled = false;

        // Load the world scene
        SceneManager.LoadScene("World");
    }
}