using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Game/Boss Stats")]
public class BossStats : ScriptableObject
{
    public int maxHp = 20;
    
    public float distance = 0.4f;
}
