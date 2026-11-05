// // using UnityEngine;

// // public class EnemyShooter : MonoBehaviour
// // {
// //     public GameObject bulletPrefab;
// //     public Transform firePoint;          // optional: empty child where bullets spawn
// //     public float fireInterval = 2f;      // seconds between volleys
// //     public float bulletSpeed = 6f;
// //     public int bulletCount = 3;          // shots per volley
// //     public float spreadAngle = 20f;      // degrees between neighbouring shots

// //     private Transform player;

// //     void Start()
// //     {
// //         GameObject p = GameObject.FindGameObjectWithTag("Player");
// //         if (p != null) player = p.transform;

// //         InvokeRepeating(nameof(Shoot), 1f, fireInterval);
// //     }

// //     void Shoot()
// //     {
// //         if (player == null || bulletPrefab == null) return;

// //         Vector3 origin = firePoint != null ? firePoint.position : transform.position;
// //         Vector2 toPlayer = (player.position - origin).normalized;
// //         float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

// //         for (int i = 0; i < bulletCount; i++)
// //         {
// //             // with 3 bullets: -spread, 0, +spread around the aim direction
// //             float offset = (i - (bulletCount - 1) / 2f) * spreadAngle;
// //             float a = (baseAngle + offset) * Mathf.Deg2Rad;
// //             Vector2 dir = new Vector2(Mathf.Cos(a), Mathf.Sin(a));

// //             GameObject b = Instantiate(bulletPrefab, origin, Quaternion.identity);
// //             b.GetComponent<Rigidbody2D>().linearVelocity = dir * bulletSpeed;
// //         }
// //     }
// // }




// using UnityEngine;

// public class EnemyShooter : MonoBehaviour
// {
//     public GameObject projectilePrefab;
//     public float distance = 0.7f;

//     BossHealthBar bossHealth;

//     void Start()
//     {
//         bossHealth = GetComponent<BossHealthBar>();
//         InvokeRepeating(nameof(Shoot), 1f, 2f);
//     }

//     void Shoot()
//     {
//         int bulletCount;

//         if (bossHealth.hp > 4)
//         {
//             bulletCount = 3;
//         }
//         else
//         {
//             bulletCount = 5;
//         }

//         for (int i = 0; i < bulletCount; i++)
//         {
//             float y = (i - (bulletCount - 1) / 2f) * distance;

//             Vector3 position = transform.position
//                 + Vector3.left * 1.5f
//                 + Vector3.up * y;

//             Instantiate(projectilePrefab, position, Quaternion.identity);
//         }
//     }
// }