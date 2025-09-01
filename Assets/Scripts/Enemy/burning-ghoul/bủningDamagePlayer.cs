using UnityEngine;

public class bủningDamagePlayer : MonoBehaviour
{
    [SerializeField] private int damageAmount;

    public void damage(GameObject player)
    {
        if (player.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = player.GetComponent<PlayerHealthController>();

            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damageAmount);
            }
        }
    }
}
