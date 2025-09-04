using UnityEngine;

public class DamagePlayer : MonoBehaviour // damage gây ra cho player
{
    [SerializeField] private int damageAmount;
    [SerializeField] private PlayerHealthController playerHealth;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.gameObject.GetComponentInParent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damageAmount);
            }
            else 
            {
                Debug.Log("ko tim thay");
            }
        }
    }

    
}
