using UnityEngine;

public class Food : MonoBehaviour
{
    public float hungerRestore = 20f;

    public void Consume(PlayerController player)
    {
        player.Eat(hungerRestore);
        Destroy(gameObject);
    }
}