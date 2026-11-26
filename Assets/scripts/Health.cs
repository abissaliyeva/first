using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Health : MonoBehaviour
{
    public PlayerStats playerStats;
    
    public TextMeshProUGUI healthText; 
    private int hp = 3;

    bool invincible;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
        MusicManager.Instance.PlayHit();

        if (hp <= 0)
        {
            StartCoroutine(DieAndRestart());
            return;             
        }

        StartCoroutine(Invincibility());
    }

    public void Heal(int amount)
    {
        hp += amount;
        healthText.text = "Health: " + hp;
        MusicManager.Instance.PlayHeal();
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Game Over");
    }

    private IEnumerator DieAndRestart()
    {
        MusicManager.Instance.PlayLose();
        yield return new WaitForSecondsRealtime(0.8f);
        GameOver();
    }
}