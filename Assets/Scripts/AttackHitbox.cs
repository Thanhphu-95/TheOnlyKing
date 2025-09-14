using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private int damage = 5;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayerMask;

    private void Update()
    {
           
    }
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    public void DoAttack()
    {
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayerMask);
        foreach (Collider2D hitCollider in hitEnemy)
        {
            Debug.Log($"gây damage");
            EnemyHealthController enemyHealth = hitCollider.GetComponentInParent<EnemyHealthController>();
            BossHealthController ghostBoss = hitCollider.GetComponentInParent<BossHealthController>();
            if (enemyHealth != null)
            {
                enemyHealth.DamageEnemy(damage);
            }

            if (ghostBoss != null)
            {
                ghostBoss.TakeDamage(damage);
            }

        } 
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }


}
