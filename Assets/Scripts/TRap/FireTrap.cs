using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown; 
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireball;
    private float cooldownTimer;
    private void Attack()
    {
        cooldownTimer = 0;
        fireball[FindFireBall()].transform.position = firePoint.position;
        //fireball[FindFireBall()].GetComponent<EnemyProjectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireBall()
    {
        for(int i = 0; i <= fireball.Length; i++ )
        {
            if (!fireball[i].activeInHierarchy)
            {
                return i;
            }
        } 
        return 0;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }

}
