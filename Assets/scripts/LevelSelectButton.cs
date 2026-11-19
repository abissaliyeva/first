
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectButton : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int levelNumber = 1;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI starsText;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(PlayLevel);
    }

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        if (SaveManager.Instance != null)
            Refresh();
    }

    public void Refresh()
    {
        if (button == null)
            return;

        if (SaveManager.Instance == null)
        {
            button.interactable = false;
            return;
        }

        bool unlocked =
            SaveManager.Instance.IsLevelUnlocked(levelNumber);

        button.interactable = unlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);

        if (starsText != null)
        {
            int stars =
                SaveManager.Instance.GetBestStars(levelNumber);

            starsText.text = stars + " / 3";
        }
    }

    public void PlayLevel()
    {
        if (SaveManager.Instance == null)
            return;

        if (!SaveManager.Instance.IsLevelUnlocked(levelNumber))
        {
            Debug.Log("Level " + levelNumber + " is locked!");
            return;
        }

        GameSession.levelIndex = levelNumber - 1;

        Time.timeScale = 1f;

        SceneManager.LoadScene("Game");
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(PlayLevel);
    }
}
