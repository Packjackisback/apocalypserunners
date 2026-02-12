using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int currentStep = 0;

    void Start()
    {
        GameEvents.TutorialStarted();
        GameEvents.TutorialStepChanged(currentStep);
    }

    void Update()
    {
        // Click anywhere (mouse or spacebar) to advance
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        currentStep++;

        if (currentStep > 4) // end of tutorial
        {
            GameEvents.TutorialCompleted();
        }
        else
        {
            GameEvents.TutorialStepChanged(currentStep);
        }
    }
}
