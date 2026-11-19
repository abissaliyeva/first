using UnityEngine;

public enum LevelGoal
{
    CollectCoins,
    ReachExit,
    DefeatEnemies
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public Vector3 spawnPosition;
}

[CreateAssetMenu(
    fileName = "Level_01",
    menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Level")]
    public int levelNumber = 1;
    public string levelName = "Level 1";
    public LevelGoal goal = LevelGoal.CollectCoins;

    [Header("Layout")]
    public GameObject layoutPrefab;
    public Vector3 playerSpawnPosition;
    public Vector3 exitPosition;

    [Header("Coins")]
    public GameObject coinPrefab;
    public Vector3[] coinPositions;
    public int coinsRequired = 6;

    [Header("Enemies")]
    public EnemySpawn[] enemies;

    [Header("Time")]
    public float timeLimit = 120f;
    public float threeStarGoal = 30f;
    public float twoStarGoal = 60f;

    public int GetStars(float timeUsed)
    {
        if (timeUsed <= threeStarGoal)
            return 3;

        if (timeUsed <= twoStarGoal)
            return 2;

        return 1;
    }
}
