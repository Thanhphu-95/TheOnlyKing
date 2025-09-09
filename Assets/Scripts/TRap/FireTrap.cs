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
    [SerializeField]private Direction fireDirection;

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
        float angle = 0f;
        switch (fireDirection)
        {
            case Direction.up: angle = -90f; break;
            case Direction.down: angle = 90f; break;
            case Direction.left: angle = 0f; break;
            case Direction.right: angle = 180f; break;
        }
        for (int i = 0; i < burstCount; i++) 
        {
            FireControler fireControler = Instantiate(firePrefab, shootPosition.position, Quaternion.Euler(0,0,angle)); 
            yield return new WaitForSeconds(burstDelay);
        }
        timer = cooldown;
        iShooting =false;

    }

}

public enum Direction
{
    up, down, left, right
}