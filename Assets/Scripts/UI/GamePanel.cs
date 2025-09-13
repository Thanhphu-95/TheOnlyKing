using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private GameObject bossHealth;

    private int playerMaxHealth;

    public void SetMaxHealth(int maxHealth)
    {
        playerMaxHealth = maxHealth;
        playerHealthSlider.maxValue = maxHealth;
    }
    public void UpdateHealth(int currentHealth)
    {
        playerHealthSlider.value = currentHealth;
    }

    public void ResetHealth()
    {
        UpdateHealth(playerMaxHealth);
    }


    public void SetBossMaxHealth(int maxHealth)
    {
        bossHealthSlider.maxValue = maxHealth;
    }
    public void UpdateBossHealth(int currentHealthValue)
    {
        bossHealthSlider.value = currentHealthValue;
    }
    public void ActiveBossHealth(bool status)
    {
        bossHealth.SetActive(status);
    }
}
