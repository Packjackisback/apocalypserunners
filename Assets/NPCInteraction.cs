using UnityEngine;
using System.Collections;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    public GameObject dialogueUI;
    public float delayBeforeWorldStart = 5f; // 5 seconds delay
    private PlayerController player;
    private bool dialogueOpen = false;

    public bool IsTutorialInteraction => false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        dialogueUI.SetActive(false);
    }

    public void Interact()
    {
        if (!dialogueOpen)
        {
            dialogueOpen = true;
            dialogueUI.SetActive(true);

            // Hide player sprite immediately
            if (player != null)
            {
                player.GetComponent<SpriteRenderer>().enabled = false;
            }

            // Start the delayed world spawning
            StartCoroutine(SpawnWorldAfterDelay());
        }
    }

    private IEnumerator SpawnWorldAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeWorldStart);

        // Hide dialogue UI
        dialogueUI.SetActive(false);

        // Restore player sprite (optional, or leave hidden)
        if (player != null)
        {
            player.GetComponent<SpriteRenderer>().enabled = true;
        }

        // Tell WorldGenerator to start spawning
        WorldGenerator worldGen = FindObjectOfType<WorldGenerator>();
        if (worldGen != null)
        {
            worldGen.StartWorld(); // starts zombies, food, and timer
        }

        // Destroy NPC now
        Destroy(gameObject);
    }
}