using UnityEngine;

public class Boss : MonoBehaviour
{
    public BossStats bossStats;
    
    public int hp;
    public BossHealthBar bar;
    public Animator exitAnimator;
    public Animator animator;

    private int maxHp = 20;

    void Start()
    {
        hp = maxHp;
        bar.SetValue(1f);
        Debug.Log("Boss HP: " + hp);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(2);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        hp = Mathf.Max(hp - dmg, 0);
        bar.SetValue((float)hp / maxHp);

        if (hp <= 0) Die();
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        exitAnimator.SetBool("bossDead", true);
        bar.gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }
}
