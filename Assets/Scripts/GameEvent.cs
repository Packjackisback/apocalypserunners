using System;

public static class GameEvent
{
    // Tutorial Events
    public static event Action<int> OnTutorialStepChanged;
    public static event Action OnTutorialStarted;
    public static event Action OnTutorialCompleted;

    // Scene Events
    public static event Action OnLoadTutorialScene;
      public static event Action OnLoadDeathScene;
    

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

    public static void LoadTutorialScene()
    {
        OnLoadTutorialScene?.Invoke();
    }
    public static void LoadDeathScene()
    {
        OnLoadDeathScene?.Invoke();
    }
}
