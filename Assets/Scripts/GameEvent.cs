using System;

public static class GameEvents
{
    // Tutorial Events
    public static event Action<int> OnTutorialStepChanged;
    public static event Action OnTutorialStarted;
    public static event Action OnTutorialCompleted;

    public static void TutorialStarted()
    {
        OnTutorialStarted?.Invoke();
    }

    public static void TutorialStepChanged(int step)
    {
        OnTutorialStepChanged?.Invoke(step);
    }

    public static void TutorialCompleted()
    {
        OnTutorialCompleted?.Invoke();
    }
}
