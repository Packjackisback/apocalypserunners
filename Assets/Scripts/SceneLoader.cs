using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvent.OnLoadTutorialScene += LoadTutorial;
        GameEvent.OnLoadDeathScene += LoadDeath;
        GameEvent.OnLoadVictoryScene += LoadVictory;
    }

    private void OnDisable()
    {
        GameEvent.OnLoadTutorialScene -= LoadTutorial;
        GameEvent.OnLoadDeathScene -= LoadDeath;
        GameEvent.OnLoadVictoryScene -= LoadVictory;
    }

    void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialScreen"); 
    }
    void LoadDeath()
    {
        SceneManager.LoadScene("DeathScreen");
    }
    void LoadVictory()
    {
        SceneManager.LoadScene("VictoryScreen");
    }
}
