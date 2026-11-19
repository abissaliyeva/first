
using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private int totalLevels = 3;

    public SaveData Data { get; private set; }

    private string SavePath =>
        Path.Combine(
            Application.persistentDataPath,
            "save.json"
        );

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Load()
    {
        Data = new SaveData();

        if (!File.Exists(SavePath))
        {
            Debug.Log("New game: default save loaded.");
            return;
        }

        try
        {
            string json = File.ReadAllText(SavePath);

            SaveData loaded =
                JsonUtility.FromJson<SaveData>(json);

            if (loaded == null ||
                loaded.version != 1 ||
                loaded.unlockedLevel < 1 ||
                loaded.unlockedLevel > totalLevels ||
                loaded.results == null ||
                float.IsNaN(loaded.musicVolume) ||
                loaded.musicVolume < 0f ||
                loaded.musicVolume > 1f)
            {
                throw new Exception("Invalid save data");
            }

            foreach (LevelResult result in loaded.results)
            {
                if (result == null ||
                    result.levelNumber < 1 ||
                    result.levelNumber > totalLevels ||
                    result.bestStars < 0 ||
                    result.bestStars > 3 ||
                    float.IsNaN(result.bestTime) ||
                    result.bestTime < 0f ||
                    result.bestScore < 0)
                {
                    throw new Exception("Invalid level result");
                }
            }

            Data = loaded;

            Debug.Log("Progress loaded successfully!");
        }
        catch (Exception e)
        {
            Debug.LogWarning(
                "Save is corrupted. Loading defaults: " +
                e.Message
            );

            Data = new SaveData();
        }
    }

    public void Save()
    {
        if (Data == null)
            return;

        try
        {
            string json = JsonUtility.ToJson(Data, true);

            File.WriteAllText(SavePath, json);

            Debug.Log("Progress saved: " + SavePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Save failed: " + e.Message);
        }
    }

    public void CompleteLevel(
        int levelNumber,
        int stars,
        float time,
        int score = 0)
    {
        if (Data == null ||
            levelNumber < 1 ||
            levelNumber > totalLevels)
            return;

        stars = Mathf.Clamp(stars, 1, 3);
        time = Mathf.Max(0f, time);
        score = Mathf.Max(0, score);

        LevelResult result = Data.results.Find(
            r => r.levelNumber == levelNumber
        );

        if (result == null)
        {
            result = new LevelResult
            {
                levelNumber = levelNumber,
                bestStars = stars,
                bestTime = time,
                bestScore = score
            };

            Data.results.Add(result);
        }
        else
        {
            result.bestStars =
                Mathf.Max(result.bestStars, stars);

            if (result.bestTime <= 0f ||
                time < result.bestTime)
            {
                result.bestTime = time;
            }

            result.bestScore =
                Mathf.Max(result.bestScore, score);
        }

        Data.unlockedLevel = Mathf.Max(
            Data.unlockedLevel,
            Mathf.Min(totalLevels, levelNumber + 1)
        );

        Save();
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return Data != null &&
               levelNumber >= 1 &&
               levelNumber <= totalLevels &&
               levelNumber <= Data.unlockedLevel;
    }

    public int GetBestStars(int levelNumber)
    {
        if (Data == null)
            return 0;

        LevelResult result = Data.results.Find(
            r => r.levelNumber == levelNumber
        );

        return result != null ? result.bestStars : 0;
    }

    public void SetMusicVolume(float volume)
    {
        Data.musicVolume = Mathf.Clamp01(volume);
        Save();
    }

    public void ResetProgress()
    {
        Data = new SaveData();
        Save();
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
            Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
