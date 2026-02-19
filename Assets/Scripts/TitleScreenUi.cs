using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        GameEvent.LoadTutorialScene();
        Debug.Log("Clicked");
    }
}
