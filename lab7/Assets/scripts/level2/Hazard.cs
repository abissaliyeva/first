using UnityEngine;

public class Hazard : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D c)
    {
        if (!c.gameObject.CompareTag("Player")) return;
        OnHitPlayer(c.gameObject.GetComponent<Health>());
        Destroy(gameObject);
    }

    // virtual = наследник МОЖЕТ переопределить (в Java так по умолчанию)
    protected virtual void OnHitPlayer(Health player)
    {
        player.TakeDamage(1);
    }
}

