
using UnityEngine;

public static class GameSession
{
    public static int levelIndex = 0;
}

public class LevelLoader : MonoBehaviour
{
    [Header("Levels")]
    [SerializeField] private LevelData[] levels;


    [Header("Scene Objects")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform exitDoor;

    public static LevelData CurrentLevel { get; private set; }
    public static int LevelCount { get; private set; }

    
    private void Awake()
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogError("No LevelData assets assigned!");
            return;
        }

        LevelCount = levels.Length;

        int index = GameSession.levelIndex;

        Debug.Log("Selected level index: " + index);

        if (index < 0 || index >= levels.Length)
        {
            Debug.LogError("Invalid level index: " + index);
            return;
        }

        CurrentLevel = levels[index];

        if (CurrentLevel == null)
        {
            Debug.LogError("LevelData is missing at index " + index);
            return;
        }

        Debug.Log("Loading LevelData: " + CurrentLevel.levelName);

        LoadLevel(CurrentLevel);
    }


    private void LoadLevel(LevelData data)
    {
        Debug.Log("Loading " + data.levelName);

        if (data.layoutPrefab != null)
        {
            Instantiate(data.layoutPrefab);
        }

        if (player != null)
        {
            player.position = data.playerSpawnPosition;
        }

        if (exitDoor != null)
        {
            exitDoor.position = data.exitPosition;
        }

        if (data.coinPrefab != null &&
            data.coinPositions != null)
        {
            foreach (Vector3 position in data.coinPositions)
            {
                Instantiate(
                    data.coinPrefab,
                    position,
                    Quaternion.identity
                );
            }
        }

        if (data.enemies != null)
        {
            foreach (EnemySpawn enemy in data.enemies)
            {
                if (enemy == null ||
                    enemy.enemyPrefab == null)
                    continue;

                Instantiate(
                    enemy.enemyPrefab,
                    enemy.spawnPosition,
                    Quaternion.identity
                );
            }
        }

        if (data.goal == LevelGoal.CollectCoins &&
            data.coinPositions != null &&
            data.coinsRequired != data.coinPositions.Length)
        {
            Debug.LogWarning(
                "Required coins differ from spawned coins!"
            );
        }

        Debug.Log("Level loaded successfully!");
    }
}
