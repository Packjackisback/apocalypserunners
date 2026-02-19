using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int currentStep = 0;

    void Start()
    {
        GameEvent.TutorialStarted();
        GameEvent.TutorialStepChanged(currentStep);
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
            GameEvent.TutorialCompleted();
        }
        else
        {
            GameEvent.TutorialStepChanged(currentStep);
        }
    }
}
