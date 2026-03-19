using UnityEngine;

public class DeathScreenUI : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        GameEvent.LoadTutorialScene();
        Debug.Log("Clicked");
    }
}
