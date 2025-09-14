using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private int damage = 10;       // Sát thương gây ra
    [SerializeField] private bool repeatDamage = true; // Nếu player đứng trên trap vẫn nhận damage liên tục
    [SerializeField] private float damageTimer = 1f; // Khoảng thời gian giữa các lần trúng damage
    [SerializeField] private PlayerHealthController playerHealth;



    private float timer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter2D: " + collision.name + " tag=" + collision.tag);
            Debug.Log("dinh bay");
        

            if (playerHealth != null)
            {
                Debug.Log("mat mau");
                playerHealth.DamagePlayer(damage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!repeatDamage) return;

        if (collision.CompareTag("Player"))
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                
                if (playerHealth != null)
                {
                    playerHealth.DamagePlayer(damage);
                    timer = damageTimer;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timer = 0f; // reset timer khi player rời trap
        }
    }
}
