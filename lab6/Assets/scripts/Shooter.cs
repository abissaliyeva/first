using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.3f;
    public AudioClip audioClip;
    float nextShot;
    private AudioSource audioSource;

    SpriteRenderer sr;

    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.X) && Time.time >= nextShot)
        {
            nextShot = Time.time + cooldown;
            audioSource.PlayOneShot(audioClip);
            // If the sprite is flipped, rotate the bullet 180° so its "right" points left
            Quaternion rot = sr.flipX ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
            Instantiate(bulletPrefab, transform.position, rot);
        }
    }
}