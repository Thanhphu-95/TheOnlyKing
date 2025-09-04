using System.Collections;
using UnityEngine;

public class BurningGhoulController : MonoBehaviour
{
    [SerializeField] private Transform[] patroPoints;

    private int currentPoint;
    private float waitCounter;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedAttack;

    [SerializeField] private float timeWaitAtPoint;
    [SerializeField] private Rigidbody2D burningRb;
    [SerializeField] private Animator burningAnimator;

   
    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private int damageAmount ;
    [SerializeField] private GameObject burnEff;
    private bool attack = false;
    private bool exploded = false;

    private PlayerController playerController;
    [SerializeField] private float explosionRadius;//pham vi no


    void Start()
    {
        waitCounter = timeWaitAtPoint;

        foreach (Transform pPoint in patroPoints)
        {
            pPoint.SetParent(null);
        }
    }
    void Update()
    {
        if (attack == true)
        {
            StartCoroutine(ExplodeAfterDelay(5f));
            MoveAttack();
        }
        else
        {
            Patro();
        }

        if (playerController != null)
        {
            Debug.Log($"{playerController.IsDash}");
        }
    }
    private void Patro()
    {
        if (patroPoints.Length > 0)
        {

            if (Mathf.Abs(transform.position.x - patroPoints[currentPoint].position.x) > 0.2f)
            {
                if (transform.position.x < patroPoints[currentPoint].position.x)
                {
                    burningRb.linearVelocity = new Vector2(moveSpeed, burningRb.linearVelocityY);
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    burningRb.linearVelocity = new Vector2(-moveSpeed, burningRb.linearVelocityY);
                    transform.localScale = Vector3.one;
                }
            }
            else 
            {
                burningRb.linearVelocity = new Vector2(0, burningRb.linearVelocityY);
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    waitCounter = timeWaitAtPoint;
                    currentPoint++;
                    if (currentPoint >= patroPoints.Length)
                    {
                        currentPoint = 0;
                    }
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            attack = true;  
        }
    }

    private void MoveAttack()
    {
        Vector2 targetPosition = new Vector2(targetPlayer.transform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeedAttack * Time.deltaTime);


        if (targetPlayer.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (targetPlayer.transform.position.x < transform.position.x)
        {
            transform.localScale = Vector3.one;
        }


        if (targetPlayer != null && !exploded)
        {
            if (Mathf.Abs(targetPlayer.transform.position.x - transform.position.x) <= 1 && Mathf.Abs(targetPlayer.transform.position.y - transform.position.y) <= 1)
            {
                exploded = true;
                StartCoroutine(ExplodeAfterDelay(0.3f));
            } 
        }
    }

    private void PlayerDash()
    {
        
    }    
    private IEnumerator ExplodeAfterDelay(float delay)
    {   
       
        yield return new WaitForSeconds(delay);
        if (burnEff != null)
        {
            GameObject eff = Instantiate(burnEff, transform.position, transform.rotation);
            Destroy(eff, 1f);
        }
        Destroy(this.gameObject);

   

        float distance = Vector2.Distance(transform.position, targetPlayer.transform.position);

        if (distance <= explosionRadius)
        {
            PlayerHealthController damagePlayer = targetPlayer.GetComponent<PlayerHealthController>();
            if (damagePlayer != null)
            {
                damagePlayer.DamagePlayer(damageAmount);
            }
        }
    }
}

//Dash vẫn chưa xuyên enemy khi lướt, chưa đổi hình khi lướt
