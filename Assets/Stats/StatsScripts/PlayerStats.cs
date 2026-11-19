using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public float speed = 7f;
    public float jumpForce = 6f;
    
    public int hp = 3;

    public float cooldown = 0.3f;
    
    public float goal = 10f;
    
}
