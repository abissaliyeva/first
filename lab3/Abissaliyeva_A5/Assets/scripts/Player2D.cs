using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2D : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 7f;
    

    [Header("Health")]
    public int maxHealth = 3;
    public float invincibleTime = 1f;   

    [Header("UI (drag these in the Inspector)")]
    public GameObject winPanel;           
    public TextMeshProUGUI messageText;    
    public TextMeshProUGUI coinsText;     
    public TextMeshProUGUI healthText;    

    private Rigidbody2D rb;
    private int health;
    private float lastHitTime = -999f;

    public Animator animator;
    public Animator portalAnimator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private int coins;
    private int coinsTotal;

    void Start()
    {
        Time.timeScale = 1f;

        if (winPanel != null) winPanel.SetActive(false);
        if (messageText != null) messageText.gameObject.SetActive(false);

        health = maxHealth;
        coinsTotal = GameObject.FindGameObjectsWithTag("Coin").Length;
        coinsText.text = "Coins: 0 / " + coinsTotal;
        healthText.text = "Health: " + health;
        Debug.Log("Total coins in scene: " + coinsTotal);

        UpdateUI();
    }
    
    public float jumpForce = 6f;
    private bool onGround;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJump", true);
            onGround = false;
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        if (x != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        } 
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")){
            onGround = true;
            animator.SetBool("isJump", false);
        }
        if (other.gameObject.CompareTag("Enemy"))
            TakeDamage(1);
    }


    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coins++;
            coinsText.text = "Coins: " + coins + " / " + coinsTotal;
            if (coinsTotal > 0 && coins >= coinsTotal)
            {
                portalAnimator.SetBool("IsEnd", true);
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
    }

    void TakeDamage(int amount)
    {
        if (Time.time - lastHitTime < invincibleTime) return;
        lastHitTime = Time.time;

        health -= amount;
        healthText.text = "Health: " + health;
        UpdateUI();

        if (health <= 0)
        {
            Debug.Log("Game over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void UpdateUI()
    {
        if (coinsText != null) coinsText.text = "Coins: " + coins + " / " + coinsTotal;
        if (healthText != null) healthText.text = "Health: " + health;
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
}