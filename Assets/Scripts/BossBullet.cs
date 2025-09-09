using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damageAmount;
    [SerializeField] private float timeTodestroy;


    void Start()
    {
      
        Vector3 direction =(transform.position - player.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Destroy(gameObject, timeTodestroy);
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = -transform.right * moveSpeed;
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

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
