using UnityEngine;
using TMPro;

public class WorldGenerator : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject zombiePrefab;

    public float spawnRadius = 50f;
    public float foodSpawnTime = 2f;
    public float zombieSpawnTime = 3f;

    public float winTime = 120f;
    public TextMeshProUGUI winTimerText;

    private Transform player;
    private float timer = 0f;
    private bool hasWon = false;
    private bool spawningStarted = false;
    private bool timerActive = false; // NEW flag

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        GameEvent.OnTutorialCompleted += StartWorld;
    }

    void OnDestroy()
    {
        GameEvent.OnTutorialCompleted -= StartWorld;
    }

   public void StartWorld()
{
    if (spawningStarted) return;

    spawningStarted = true;
    timerActive = true; // start timer immediately

    // Start spawning
    InvokeRepeating("SpawnFood", 1f, foodSpawnTime);
    InvokeRepeating("SpawnZombie", 1f, zombieSpawnTime);

    // Start player hunger
    PlayerController playerController = player.GetComponent<PlayerController>();
    if (playerController != null)
    {
        playerController.StartHungerDrain();
        playerController.hasTalkedToNPC = true; // this ensures the timer runs
    }

    Debug.Log("World spawning started");
}

    void Update()
    {
        if (!hasWon && timerActive)
        {
            timer += Time.deltaTime;

            if (winTimerText != null)
            {
                float timeLeft = Mathf.Max(winTime - timer, 0f);
                winTimerText.text = $"Survive: {timeLeft:F1}s";
            }

            if (timer >= winTime)
            {
                hasWon = true;
                WinGame();
            }
        }
    }

    void SpawnFood()
    {
        if (player == null) return;

        Vector2 pos = RandomSpawnPosition();
        Instantiate(foodPrefab, pos, Quaternion.identity);
    }

    void SpawnZombie()
    {
        if (player == null) return;

        Vector2 pos = RandomSpawnPosition();
        GameObject zombie = Instantiate(zombiePrefab, pos, Quaternion.identity);

        ZombieController zController = zombie.GetComponent<ZombieController>();
        if (zController != null)
            zController.target = player.transform;
    }

    Vector2 RandomSpawnPosition()
    {
        Vector2 random = Random.insideUnitCircle.normalized * spawnRadius;
        return (Vector2)player.position + random;
    }

    void WinGame()
    {
        Debug.Log("You Win!");
        GameEvent.LoadVictoryScene();
    }
}