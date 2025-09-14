using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject deadEff;
    [SerializeField] private Slider EnemySlider;
    [SerializeField] private Animator animatorMinotaur;
    private bool isDead = false;
    private int currentHealth;


    private int NimoDead = Animator.StringToHash("Dead");
    private void Start()
    {
        currentHealth = maxHealth;

        if (EnemySlider)
        {
            EnemySlider.maxValue = maxHealth;
            EnemySlider.value = currentHealth;
        }
    }
    public void DamageEnemy(int damage)
    {
        if(isDead) return;
      
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            isDead = true;
        }

        if (EnemySlider != null)
        {
            EnemySlider.value = currentHealth;  
        }
    }

    private void Die()
    {
        

        if (animatorMinotaur != null)
        {
            animatorMinotaur.SetTrigger(NimoDead); // chạy animation Die
        }

        if (deadEff != null)
        {
            Instantiate(deadEff, transform.position, transform.rotation);
        }

        // Hủy object sau khi animation kết thúc (ví dụ 2 giây)
        Destroy(gameObject);
    }
}
