using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    public PlayerStats playerStats;  
    public GameObject winPanel;           
    public TextMeshProUGUI messageText;    
    public TextMeshProUGUI coinsText;    
    public bool coinCollected = false;  

    private Rigidbody2D rb;

    public Animator animator;
    public Animator portalAnimator;
    public AudioClip coinCollectSound;
    public GameObject settingsPanel;

    private AudioSource audioSource;
    private float speed = 7f;
    private float jumpForce = 6f;

    private bool onGround;
    private bool levelCompleted = false;


    SpriteRenderer sr;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        speed = playerStats.speed;
    }


    private int coins;
    private int coinsTotal;

    void Start()
    {
        StartCoroutine(ShowMessage("Collect all the coins to win! " + coins + " / " + coinsTotal));
        Time.timeScale = 1f;

        if (winPanel != null) winPanel.SetActive(false);
        if (messageText != null) messageText.gameObject.SetActive(false);

        if(coinsText != null)
        {
            coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
            coinsText.text = "Coins: 0 / " + coinsTotal;
            Debug.Log("Total coins in scene: " + coinsTotal);
        }

        UpdateUI();
    }

    void Update()
    {
        if(transform.position.y < 204.678)
        {
            Restart();
        }
    }

    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")){
            onGround = true;
            animator.SetBool("IsJump", false);
        }
        if (other.gameObject.CompareTag("Enemy"))
            gameObject.GetComponent<Health>().TakeDamage(1);
    }


    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            audioSource.PlayOneShot(coinCollectSound);
            coinsText.text = "Coins: " + coins + " / " + coinsTotal;
            if (coinsTotal > 0 && coins >= coinsTotal)
            {
                coinCollected = true;
                portalAnimator.SetBool("coinCollected", true);
            }
            UpdateUI();
        }

        
        if (other.CompareTag("Exit"))
        {
            if (coins >= coinsTotal && !levelCompleted)
            {
                LevelData level = LevelLoader.CurrentLevel;

                if (level == null)
                {
                    Debug.LogError("LevelData is missing!");
                    return;
                }

                if (SaveManager.Instance == null)
                {
                    Debug.LogError("SaveManager is missing!");
                    return;
                }

                levelCompleted = true;

                // Calculate the time spent on this level
                float elapsedTime = Time.timeSinceLevelLoad;

                // Calculate earned stars from LevelData
                int earnedStars = level.GetStars(elapsedTime);

                // Save progress and unlock the next level
                SaveManager.Instance.CompleteLevel(
                    level.levelNumber,
                    earnedStars,
                    elapsedTime
                );

                Debug.Log(
                    "Level " + level.levelNumber +
                    " completed! Stars: " + earnedStars
                );

                Debug.Log(
                    "Unlocked Level: " +
                    SaveManager.Instance.Data.unlockedLevel
                );

                // Show victory screen
                winPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else if (!levelCompleted)
            {
                StopAllCoroutines();

                StartCoroutine(ShowMessage(
                    "Collect all the coins! " +
                    coins + " / " + coinsTotal
                ));
            }
        }


        
        if (other.CompareTag("BulletBoss"))
        {
            gameObject.GetComponent<Health>().TakeDamage(1);
        }
    }

    void UpdateUI()
    {
        if (coinsText != null) coinsText.text = "Coins: " + coins + " / " + coinsTotal;
    }

    IEnumerator ShowMessage(string text)
    {
        messageText.text = text;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        messageText.gameObject.SetActive(false);
    }




    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    
    
    public void nextLevel()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager missing!");
            return;
        }

        if (LevelLoader.CurrentLevel == null)
        {
            Debug.LogError("Current LevelData missing!");
            return;
        }

        int currentIndex = GameSession.levelIndex;
        int nextIndex = currentIndex + 1;

        Debug.Log("Current Level: " + (currentIndex + 1));
        Debug.Log("Next Level: " + (nextIndex + 1));
        Debug.Log("Total Levels: " + LevelLoader.LevelCount);
        Debug.Log("Unlocked Level: " +
            SaveManager.Instance.Data.unlockedLevel);

        if (nextIndex >= LevelLoader.LevelCount)
        {
            Debug.Log("All levels completed!");
            return;
        }

        if (!SaveManager.Instance.IsLevelUnlocked(nextIndex + 1))
        {
            Debug.LogWarning("Next level is locked!");
            return;
        }

        GameSession.levelIndex = nextIndex;

        Time.timeScale = 1f;

        Debug.Log("Loading Level " + (nextIndex + 1));

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }




    public void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void clickSettings()
    {
        Time.timeScale = 0f;
        settingsPanel.SetActive(true);
    }

    public void closeSettings()
    {
        Time.timeScale = 1f;
        settingsPanel.SetActive(false);
    }

    
}