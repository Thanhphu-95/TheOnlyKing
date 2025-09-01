using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int currentHealth; //mau hien tai

    [SerializeField] private int maxHealth; //mau toi da
    [SerializeField] private float invicibilityTime; // thoi gian bat tu sau khi trung don
    private float invicCouter; // luu va dem thoi gian bat tu.
    [SerializeField] private GameObject PlayerDeadEff;

    private void Start()
    {
        currentHealth = maxHealth;
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
    }

    public void IncreaseHeal(int Heal)//tang mau toi da
    {
        maxHealth += Heal;
    }

    public void DamagePlayer(int damageAmount)// nhan sat thuong
    {
        Debug.Log($"sat thuong = {damageAmount}");
        currentHealth -= damageAmount;
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentHealth = currentHealth -  1;
        }

        //if ()
        //{

        //}
    }

    private void SetMaxHealth()
    {
        currentHealth = maxHealth;
    }
}
