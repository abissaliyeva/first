using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    public float speed = 7f;
    public int maxHealth = 3;
    public float invincibleTime = 1f;   
    public GameObject winPanel;           
    public TextMeshProUGUI messageText;    
    public TextMeshProUGUI coinsText;    
    public bool coinCollected = false;  

    private Rigidbody2D rb;
    private int health;

    public Animator animator;
    public Animator portalAnimator;
    public AudioClip coinCollectSound;

    private AudioSource audioSource;



    SpriteRenderer sr;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }


    private int coins;
    private int coinsTotal;

    void Start()
    {
        StartCoroutine(ShowMessage("Collect all the coins to win! " + coins + " / " + coinsTotal));
        Time.timeScale = 1f;

        if (winPanel != null) winPanel.SetActive(false);
        if (messageText != null) messageText.gameObject.SetActive(false);

        health = maxHealth;
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
        coinsText.text = "Coins: 0 / " + coinsTotal;
        Debug.Log("Total coins in scene: " + coinsTotal);

        UpdateUI();
    }
    
    public float jumpForce = 6f;
    private bool onGround;

    
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
            if (coinsTotal > 0 && coins >= coinsTotal)
            {
                winPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ShowMessage("Collect all the coins! " + coins + " / " + coinsTotal));
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void backToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}