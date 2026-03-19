using UnityEngine;

public class VictoryScreenUI : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        GameEvent.LoadTutorialScene();
        Debug.Log("Clicked");
    }
}
