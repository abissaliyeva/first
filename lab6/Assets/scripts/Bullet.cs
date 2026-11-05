using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * 10f;
        Destroy(gameObject, 3f);      // пуля сама умрёт через 3 секунды
    }
}