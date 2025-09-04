using UnityEngine;
using System.Diagnostics;


public class BoxUnlock : MonoBehaviour
{
    [SerializeField] private Box giftBox;
    [SerializeField] private string unlockMessage;
    [SerializeField] private GameObject unlockEff;
    [SerializeField] private UnlockBoxMessage unlockBoxMessage;
    [SerializeField] private Animator animator;
    PlayerAbilityTracker abilityTracker;
    PlayerHealthController healthController;
    PlayerController damage;
    private bool canUnlock = false;
    private bool unlock = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        abilityTracker = collision.GetComponentInParent<PlayerAbilityTracker>();
        healthController = collision.GetComponentInParent<PlayerHealthController>();
        damage = collision.GetComponentInParent<PlayerController>();

        canUnlock = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canUnlock = false;
    }
    private void Update()
    {
        if (!unlock && canUnlock && Input.GetKeyDown(KeyCode.E))
        {
            Unlock();
            canUnlock = false;
            unlock = true;
        }
    }
    private void Unlock()
    {
        if (abilityTracker != null) 
        {
            switch(giftBox)
            {
                case Box.Nodamage: abilityTracker.Nodamage = true; break;
                case Box.MaxHealth: healthController.IncreaseHeal(20); break;
                case Box.MaxDamage: damage.IncreaseDamage(1); break;
                case Box.Soul: abilityTracker.Soul = true; break;
                case Box.Priest: abilityTracker.Priest = true; break;
            }
            animator.SetTrigger("Open");

            UnlockBoxMessage boxmess = Instantiate(unlockBoxMessage, transform.position, transform.rotation);
            boxmess.SetBoxMessage(unlockMessage);

        }
    }
}

public enum Box
{
    Unknow = 0,
    Priest,
    Nodamage,
    MaxHealth,
    MaxDamage,
    Soul,
}