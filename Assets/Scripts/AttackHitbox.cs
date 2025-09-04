using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private int damage = 5;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealthController enemyHealth = collision.GetComponentInParent<EnemyHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.DamageEnemy(damage);
            }

        }
    }
}
