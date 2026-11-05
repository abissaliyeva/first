using UnityEngine;

public class Bomb : Hazard
{
    public Animator animator;
    void Start()
    {
        Invoke(nameof(Explode), 2f);   // один отложенный вызов
    }

    void Explode()
    {
        animator.SetBool("isExploded", true);
        Destroy(gameObject, 1f);
    }

    protected override void OnHitPlayer(Health p) { p.TakeDamage(2); }
}