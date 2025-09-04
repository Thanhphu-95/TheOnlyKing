using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int currentHealth; //mau hien tai

    [SerializeField] private int maxHealth; //mau toi da
    [SerializeField] private float invicibilityTime; // thoi gian bat tu sau khi trung don
    private float invicCouter; // luu va dem thoi gian bat tu.
    [SerializeField] private GameObject PlayerDeadEff;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject priest;



    private int HurtParamPr = Animator.StringToHash("HurtPr");

    private void Start()
    {
        currentHealth = maxHealth;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.SetMaxHealth(maxHealth);
            UIManager.Instance.GamePanel.UpdateHealth(currentHealth);
        }
    }
    void Update()
    {
        
    }


    public void HealPlayer(int healthAmount)//moi mau
    {
        currentHealth += healthAmount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.UpdateHealth(currentHealth);
        }
    }

    public void IncreaseHeal(int Heal)//tang mau toi da
    {
        maxHealth += Heal;
    }

    public void DamagePlayer(int damageAmount)// nhan sat thuong
    {
        Debug.Log($"sat thuong = {damageAmount}");
        currentHealth -= damageAmount;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.UpdateHealth(currentHealth);
            if (priest.activeSelf &&!player.activeSelf)
            {
                Animator.SetTrigger(HurtParamPr);
            }
            

        }
        if (currentHealth <= 0)
        {
            Debug.Log("Die");
            //if ()
            //{
                
            //}

        }
  
    }

    private void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }
}
