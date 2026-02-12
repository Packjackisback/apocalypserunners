using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject hungerBarCircle;
    public GameObject healthBarCircle;

    void OnEnable()
    {
        GameEvents.OnTutorialStarted += LockPlayer;
        GameEvents.OnTutorialStepChanged += UpdateDialogue;
        GameEvents.OnTutorialCompleted += EndTutorial;
    }

    void OnDisable()
    {
        GameEvents.OnTutorialStarted -= LockPlayer;
        GameEvents.OnTutorialStepChanged -= UpdateDialogue;
        GameEvents.OnTutorialCompleted -= EndTutorial;
    }

    void LockPlayer()
    {
        dialogueText.gameObject.SetActive(true);
    }

    void UpdateDialogue(int step)
    {
        hungerBarCircle.SetActive(false);
        healthBarCircle.SetActive(false);

        switch (step)
        {
            case 0:
                dialogueText.text = "Watch out, there are a lot of zombies!";
                break;

            case 1:
                dialogueText.text = "Let's get to the car over on the right, quick!";
                break;

            case 2:
                dialogueText.text = "This is your hunger bar. It will drain over time.";
                hungerBarCircle.SetActive(true);
                break;
            case 3:
                dialogueText.text = "You can find food to restore hunger";
                hungerBarCircle.SetActive(true);
                break;
            case 4:
                dialogueText.text = "This is your health bar, don't let it reach zero";
                healthBarCircle.SetActive(true);
                break;
        }
    }

    void EndTutorial()
    {
        healthBarCircle.SetActive(false);
        dialogueText.gameObject.SetActive(false);
    }
}
