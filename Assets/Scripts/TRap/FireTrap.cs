using System.Collections;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private FireControler firePrefab;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private int burstCount = 3; // so fire ban moi loat
    [SerializeField] private float burstDelay = 0.3f; //thoi gian moi fire
    [SerializeField] private float cooldown; // thoi gian hồi giữa các loạt bắn
    private float timer;
    private bool iShooting;

    private void Start()
    {
        timer = cooldown;
    }


    private void Update()
    {   
        
        Attack();

    }
   private void Attack()
    {
        if (!iShooting)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartCoroutine(ShootBurst());
                
            }
        }
    }

    private IEnumerator ShootBurst()
    {
        iShooting = true;
        for (int i = 0; i < burstCount; i++) 
        {
            FireControler fireControler = Instantiate(firePrefab, shootPosition.position, shootPosition.rotation); 
            yield return new WaitForSeconds(burstDelay);
        }
        timer = cooldown;
        iShooting =false;

    }

}
