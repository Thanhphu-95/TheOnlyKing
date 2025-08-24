using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoint;
    [SerializeField] private Transform triggerArea;
    private int currentPoint;


    [SerializeField] private float moveSpeed; // tốc độ di chuyển
    [SerializeField] private float timeWaitAtPoint; //thời gian chờ ở point
    [SerializeField] private Rigidbody2D skeletonRb; //tham chiếu đên skeleton
    [SerializeField] private Animator animator;


    [SerializeField] public LayerMask raycastMask;
    [SerializeField] public float attackDistance; // khoản cách tối thiểu để tấng công
    [SerializeField] private float timerCooldowAttack; // thời gian giữa các lần tấng công



    private bool inRange; //kiểm tra vị tri player trong phạm vi tấng công
    private bool attackMode; // check trạng trái tấng công
    private bool cooling;
    private GameObject targetPlayer; //gắn đối tượng player khi vào khu bị phát hiện
    private float distance; // khoản cách đến player
    [Header("lưu giá trị")]
    private float attackCounter;
    private float waitCounter;

    private int AttackParam = Animator.StringToHash("Attack");


    private void Awake()
    {
         if (triggerArea != null)
        {
            triggerArea.SetParent(null); 
        }
    }

    private void Start()
    {
        waitCounter = timeWaitAtPoint;
        attackCounter = timerCooldowAttack;


        foreach (Transform pPoint in patrolPoint)
        {
            pPoint.SetParent(null);
        }

       
       
    }


    // Update is called once per frame
    void Update()
    {
        attackCounter -= Time.deltaTime;
        SkeletonLogic();
    }



    private void Patrol()
    {
        if (patrolPoint.Length > 0)
        {

            if (Mathf.Abs(transform.position.x - patrolPoint[currentPoint].position.x) > 0.2f)
            {
                if (transform.position.x < patrolPoint[currentPoint].position.x)
                {
                    skeletonRb.linearVelocity = new Vector2(moveSpeed, skeletonRb.linearVelocityY);
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    skeletonRb.linearVelocity = new Vector2(-moveSpeed, skeletonRb.linearVelocityY);
                    transform.localScale = Vector3.one;
                }
            }
            else
            {
                skeletonRb.linearVelocity = new Vector2(0, skeletonRb.linearVelocityY);
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    waitCounter = timeWaitAtPoint;
                    currentPoint++;
                    if (currentPoint >= patrolPoint.Length)
                    {
                        currentPoint = 0;
                    }

                }
            }

        }
    }




    private void OnTriggerEnter2D(Collider2D collision) // kiểm tra player đã vào khu vục phát hiện
    {
        if (collision.CompareTag("Player"))
        {
            targetPlayer = collision.gameObject;
            inRange = true;
            Debug.Log($"{inRange}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)// kiểm tra player thoát khỏi khu vục phát hiện
    {
        {
            if (collision.CompareTag("Player"))
            {
                targetPlayer = null;
                inRange = false;
                Debug.Log($"{inRange}");
            }
        }

       

    }

    void SkeletonLogic() //hành vi của Skeleton
    {
        if (targetPlayer == null)
        {
            Patrol();
            return;
        }
        distance = Vector2.Distance(transform.position, targetPlayer.transform.position); // tinhs khoản cách đến player


        if (distance > attackDistance && !inRange) // player ko có trong khu vực
        {
            Patrol();
        }
        else if (inRange)
        {
            Move();
            
            if (distance <= attackDistance && cooling == false)
            {
                
                Attack();
            }
            else
            {
                StopAttack();
            }
        }

        if (cooling)
        {
            CoolDown();
            animator.SetBool(AttackParam, false);
        }

    }


    private void Move()
    {
        Vector2 targerPosition = new Vector2(targetPlayer.transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targerPosition, moveSpeed * Time.deltaTime);

        if (targetPlayer.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        if (targetPlayer.transform.position.x < transform.position.x)
        {
            transform.localScale = Vector3.one;
        }

    } //hàm di chuyển khi phát hiện player

    private void Attack() //hàm tấn công player
    {
        Debug.Log("Attack");
        attackCounter = timerCooldowAttack;
        attackMode = true;

        animator.SetBool(AttackParam, true);
    }

    private void CoolDown() // hamf hooif chieu
    {
        attackCounter -= Time.deltaTime;

        if (attackCounter <= 0 && cooling && attackMode)
        {
            cooling = false;
            attackCounter = timerCooldowAttack;
        }
    }


    private void StopAttack() // ham yeu cau ngung tan cong
    {
        cooling = false;
        attackMode = false;
        animator.SetBool(AttackParam, false);
    }
}



