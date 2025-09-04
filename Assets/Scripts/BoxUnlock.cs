using UnityEngine;
using System.Diagnostics;


public class BoxUnlock : MonoBehaviour
{
    [SerializeField] private Box giftBox;
    [SerializeField] private string unlockMessage;
    [SerializeField] private GameObject unlockEff;
    //[SerializeField] private UnlockAbilityMessage

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerAbilityTracker abilityTracker = collision.GetComponentInParent<PlayerAbilityTracker>();
    }


}

public enum Box
{
    Unknow = 0,
    Nodamage,
    MaxHealth,
    MaxDamage,
    Soul,
}