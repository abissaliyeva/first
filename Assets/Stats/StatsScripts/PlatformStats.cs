using UnityEngine;

[CreateAssetMenu(fileName = "PlatformStats", menuName = "Game/Platform Stats")]
public class PlatformStats : ScriptableObject
{
    
    public Vector3 offset = new Vector3(4f, 0f, 0f);
    public float speed = 0.5f;
}
