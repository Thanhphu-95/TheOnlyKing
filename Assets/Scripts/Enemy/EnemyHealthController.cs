using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int totalHealth;
    [SerializeField] private GameObject deadEff;
    [SerializeField] private Slider EnemySlider;


    private void Start()
    {
        
    }
    public void DamageEnemy(int damage)
    {
        Debug.Log("Nhan damage");
        totalHealth -= damage;
        if (totalHealth <= 0)
        {
            if (deadEff != null)
            {
                Instantiate(deadEff, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
