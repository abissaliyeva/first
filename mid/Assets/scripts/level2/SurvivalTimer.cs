using UnityEngine;

public class SurvivalTimer : MonoBehaviour
{
    public float goal = 10f;
    public Animator animator;
    float t;
    public bool won;
    bool coinCollected = false;

    void Update()
    {
        t += Time.deltaTime; 
        coinCollected = GameObject.FindObjectOfType<Player2D>().coinCollected;
        if (!won && t >= goal && coinCollected)
        {
            won = true;
            animator.SetBool("IsEnd", true);
            Debug.Log("YOU WIN!");
        }
    }
}
