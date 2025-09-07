using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject deadEff;
    [SerializeField] private Slider EnemySlider;

    private int currentHealth;


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
        Debug.Log("Nhan damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (deadEff != null)
            {
                Instantiate(deadEff, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }

        if (EnemySlider != null)
        {
            EnemySlider.value = currentHealth;  
        }
    }


}
