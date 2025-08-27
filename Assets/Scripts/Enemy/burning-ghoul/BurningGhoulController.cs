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
    [SerializeField] private int damageAmount;
    [SerializeField] private GameObject burnEff;
    private bool attack = false;
    private bool exploded = false;
    private bool countingDown = false;



    private int walkSpeedParam = Animator.StringToHash("walke");


    void Start()
    {
        waitCounter = timeWaitAtPoint;

        foreach (Transform pPoint in patroPoints)
        {
            pPoint.SetParent(null);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (attack == true)
        {
            Debug.Log("chuyen trang thai");
            MoveAttack();
        }
        else
        {
            Patro();
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
        burningAnimator.SetFloat(walkSpeedParam, Mathf.Abs(burningRb.linearVelocityX));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            attack = true;
           


        //    if (!countingDown)
        //    {
        //        exploded = true; 
        //        Debug.Log($"Attack: {attack}");
        //    StartCoroutine(ExplodeAfterDelay1(5f));

      
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
            if (Mathf.Abs(targetPlayer.transform.position.x - transform.position.x) <= 5)
            {
                exploded = true;
                Debug.Log("1s");
                StartCoroutine(ExplodeAfterDelay(1f));
                
            } 
        }
        
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {   
       
        yield return new WaitForSeconds(delay);
        if (burnEff != null)
        {
            GameObject eff = Instantiate(burnEff, transform.position, transform.rotation);
            Destroy(eff, 2f);
        }
        Destroy(this.gameObject);
    }
}
