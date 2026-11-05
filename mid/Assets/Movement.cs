using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 300f;
    public TextMeshProUGUI coinText;
    public AudioSource audioSource;
    public AudioClip coinSound;

    private int coins = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        transform.Translate(x * speed * Time.deltaTime, 0, z * speed * Time.deltaTime);
        transform.Rotate(0, x * speed * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.Space))
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce);
    }

    void OnTriggerEnter(Collider coin)
    {
        Destroy(coin.gameObject);
        coins++;
        coinText.text = "Монеты: " + coins;
        audioSource.PlayOneShot(coinSound);
    }
}