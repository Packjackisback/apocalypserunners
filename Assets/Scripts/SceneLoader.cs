using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvent.OnLoadTutorialScene += LoadTutorial;
        GameEvent.OnLoadDeathScene += LoadDeath;
    }

    private void OnDisable()
    {
        GameEvent.OnLoadTutorialScene -= LoadTutorial;
        GameEvent.OnLoadDeathScene -= LoadDeath;
    }

    void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScreen"); 
    }
    void LoadDeath()
    {
        SceneManager.LoadScene("DeathScreen");
    }
}
