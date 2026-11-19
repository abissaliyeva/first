using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public PlatformStats platformStats;
    private Vector3 offset;
    private float speed = 0.5f;

    private Vector3 start;

    void Start()
    {
        start = transform.position;
        offset = platformStats.offset;
        speed = platformStats.speed;
    }

    void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.position = Vector3.Lerp(start, start + offset, t);
    }
}