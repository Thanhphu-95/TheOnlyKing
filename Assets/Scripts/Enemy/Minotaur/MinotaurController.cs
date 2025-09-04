using UnityEngine;

public class MinotaurController : MonoBehaviour
{

    [SerializeField] private Transform rayCast;
    [SerializeField] private float detectDistance = 10f;
    [SerializeField] private LayerMask playerLayer; // Layer của Player

    [SerializeField] public float attackDistance;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float timer;

    [SerializeField] Rigidbody2D MinoRb;
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject target;
    [SerializeField] Animator animator;
    private float distance;
    private bool attackMode;
    private bool inRange;
    private bool cooling;
    private float intTimer;
  
    private void Awake()
    {
        intTimer = timer;
        animator = GetComponentInChildren<Animator>();
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Vị trí bắt đầu ray
        Vector2 origin = transform.position;

        // Hướng ray 
        Vector2 direction = transform.localScale.x < 0 ? Vector2.right : Vector2.left;

        // Bắn ray
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, detectDistance, playerLayer);

        if (hit.collider != null)
        {
            target = hit.collider.gameObject;
            inRange = true;
        }

        // Vẽ ray trong Scene để debug
        Debug.DrawRay(origin, direction * detectDistance, Color.red);

        if (inRange == false)
        {
            animator.SetBool("Walk", false);
            StopAttack();
        }
        else if (inRange == true) 
        {
            EnemyLogic();
        }

        //if (cooling)
        //{
        //    Cooldown();
        //}

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
            
    //        target = collision.gameObject;
    //        inRange = true;
    //    }
    //}

    private void EnemyLogic()
    {
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (distance <= attackDistance && cooling == false)
        {
            Attack();
        }

        //if (cooling)
        //{
        //    animator.SetBool("Attack", false);
        //}

    }

    private void Move()
    {
        animator.SetBool("Walk", true);
        Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);


        if (target.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (target.transform.position.x < transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
    }
    private void Attack()
    {
        timer = intTimer;
        attackMode = true;
        cooling = true;

        animator.SetBool("Walk", false);
        animator.SetBool("Attack", true);
        MinoRb.bodyType = RigidbodyType2D.Static;
    }

    private void StopAttack()
    {
        cooling = false;
        attackMode= false;
        animator.SetBool("Attack", false);
        MinoRb.bodyType = RigidbodyType2D.Dynamic;
    }



    //private void RaycastDebugger()
    //{
    //    if (distance > attackDistance)
    //    {
    //        Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.red);
    //    }
    //    else if( distance < attackDistance)
    //    {
    //        Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.green);
    //    }
    //}
}
