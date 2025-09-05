using System;
using Unity.VisualScripting;
using UnityEngine;

public class FireControler : MonoBehaviour
{
    [SerializeField] private float fireSpeed; //toc do fire
    [SerializeField] private Vector2 fireDirection;
    [SerializeField] private Rigidbody2D fireRb;
    [SerializeField] private GameObject fireEff;
    [SerializeField] private int damageAmount;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        fireRb.linearVelocity = fireSpeed * fireDirection;
    }

    public void SetDirection(Vector2 newDirection)
    {
        fireDirection = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthController playerHealth = collision.GetComponentInParent<PlayerHealthController>();

            if (playerHealth != null)
            {
                playerHealth.DamagePlayer(damageAmount);
            }
        }
        if (fireEff != null)
        {
            GameObject eff = Instantiate(fireEff, transform.position, Quaternion.identity);
            Destroy(eff, 1f); 
        }
        
        Destroy(gameObject);
    }
}
