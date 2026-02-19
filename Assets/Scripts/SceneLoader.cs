using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvent.OnLoadTutorialScene += LoadTutorial;
    }

    private void OnDisable()
    {
        GameEvent.OnLoadTutorialScene -= LoadTutorial;
    }

    void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScreen"); // exact scene name
    }
}
