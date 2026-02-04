using UnityEngine;

public class StateManager
{
    private int _lifes = 3;
    public int Lifes { get { return _lifes; } set { _lifes = value; } }

    public static void GameOver()
    {
        Debug.Log("Game Over");
    }
}
