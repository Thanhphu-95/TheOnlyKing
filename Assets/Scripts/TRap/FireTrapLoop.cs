using UnityEngine;
using System.Collections;

public class FireTrapShooter : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField] private GameObject fireballPrefab; // Prefab quả cầu lửa
    [SerializeField] private Transform shootPoint;      // Vị trí bắn ra
    [SerializeField] private float fireballSpeed = 5f;  // Tốc độ bay
    [SerializeField] private float shootInterval = 2f;  // Thời gian giữa các lần bắn
    [SerializeField] private int damage = 10;

    private void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void Shoot()
    {
        GameObject fireball = Instantiate(fireballPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = shootPoint.right * fireballSpeed; // bắn theo hướng mũi tên shootPoint
        }

   
    }
}
