using UnityEngine;

public class Shooter : MonoBehaviour
{
    public PlayerStats PlayerStats;
    private GameObject bulletPrefab;
    
    private float cooldown = 0.3f;
    public AudioClip audioClip;
    float nextShot;
    private AudioSource audioSource;

    SpriteRenderer sr;

    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        cooldown = PlayerStats.cooldown;
    }

    public void Shoot()
    {
            nextShot = Time.time + cooldown;
            audioSource.PlayOneShot(audioClip);
            
            Quaternion rot = sr.flipX ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
            Instantiate(bulletPrefab, transform.position, rot);
    }
}