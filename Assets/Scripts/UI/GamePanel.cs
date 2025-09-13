using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider bossHealthSlider;
    [SerializeField] private GameObject bossHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    private int playerMaxHealth;

    public void SetMaxHealth(int maxHealth)
    {
        playerMaxHealth = maxHealth;
        playerHealthSlider.maxValue = maxHealth;
        healthText.text = $"{maxHealth}/{maxHealth}";
    }
    public void UpdateHealth(int currentHealth)
    {
        playerHealthSlider.value = currentHealth;
        healthText.text = $"{currentHealth}/{playerHealthSlider.maxValue}";
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
