using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damageAmount;
    [SerializeField] private float timeTodestroy;
    [SerializeField] private GameObject enemyPrefab;
    public bool spawnEnemy = false;
    public bool attack03 = false;




    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>()?.transform;
    }
    void Start()
    {
        if (!attack03)
        {
            Vector3 direction =(transform.position - player.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        

        Destroy(gameObject, timeTodestroy);
    }

    void Update()
    {
        if (!attack03)
        {
            rb.linearVelocity = -transform.right * moveSpeed;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.gameObject.GetComponentInParent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damageAmount);
            }
        }

        

        if (collision.gameObject.CompareTag("Ground") && spawnEnemy == true) //dùng cho attack03
        {
            if (enemyPrefab != null)
            {
                Vector3 spawnPos = transform.position;
                
                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                float DirX = Mathf.Sign(rb.linearVelocityX);
                enemy.transform.localScale = new Vector3(DirX, 1, 1);
            }
        }
        if (impactEffect != null)
        {
            GameObject eff = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(eff, 0.5f);
        }
        Destroy(gameObject);

    }
}
