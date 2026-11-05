using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs; 
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        // (что вызвать, через сколько первый раз, интервал)
        InvokeRepeating(nameof(Spawn), 1f, 2f);
    }

    void Spawn()
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 c = cam.transform.position;

        float x = Random.Range(c.x - halfWidth, c.x + halfWidth);
        float y = c.y + halfHeight + 1f;
        Hazard pick = prefabs[Random.Range(0, prefabs.Length)];
        Instantiate(pick, new Vector3(x, y, 0), Quaternion.identity);
    }
}