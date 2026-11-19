using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Health : MonoBehaviour
{
    public PlayerStats playerStats;
    
    public TextMeshProUGUI healthText;   
    public AudioClip damageSound;
    public AudioClip healSound;

    private AudioSource audioSource;
    private int hp = 3;

    bool invincible;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        healthText.text = "Health: " + hp;
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 10; i++)
        {
            sr.enabled = !sr.enabled;               // blink
            yield return new WaitForSeconds(0.2f);
        }
        sr.enabled = true;
        invincible = false;
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;
        hp -= dmg;
        healthText.text = "Health: " + hp;
        if(audioSource != null && damageSound != null){
            audioSource.PlayOneShot(damageSound);
        }

        if (hp <= 0)
        {
            GameOver();
            return;                                 // don't start blinking if dead
        }

        StartCoroutine(Invincibility());
    }

    public void Heal(int amount)
    {
        hp += amount;
        healthText.text = "Health: " + hp;
        if(audioSource != null && healSound != null){
            audioSource.PlayOneShot(healSound);
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game Over");
    }
}