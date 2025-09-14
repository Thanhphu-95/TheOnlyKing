using UnityEngine;
using System.Collections;
using UnityEditor;

public class MinotaurController : MonoBehaviour
{

    [SerializeField] private Transform rayCast;
    [SerializeField] private float detectDistance = 10f;
    [SerializeField] private LayerMask playerLayer; // Layer của Player
    [SerializeField] private GameObject hitBox;
    [SerializeField] private EnemyHealthController enemyHealthController;
    [SerializeField] public float attackDistance;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float timer;

    [SerializeField] Rigidbody2D MinoRb;
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject target;
    [SerializeField] Animator animator;
    private float distance;
    private bool firstattack;
    private bool inRange;
    private float intTimer;
  
    private void Awake()
    {
        intTimer = 0;
        animator = GetComponentInChildren<Animator>();
        
    }

    void Start()
    {
        hitBox.SetActive(false);

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
            if (!inRange)
            {
                firstattack = true;
            }
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

        
    }



    private void EnemyLogic()
    {
        intTimer -= Time.deltaTime;
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (distance <= attackDistance)
        {
            if (firstattack == true)
            {
                Attack();
                intTimer = timer;
                firstattack = false;
            }
            else if (!firstattack && intTimer <= 0)
            {
                
                Attack();
                intTimer = timer;
            }
            else
            {
                StopAttack();
            }
            
        }
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
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", true);
        MinoRb.bodyType = RigidbodyType2D.Static;
        StartCoroutine(EnableHitboxForSeconds(0.2f));



    }

    private void StopAttack()
    {
        animator.SetBool("Attack", false);
        MinoRb.bodyType = RigidbodyType2D.Dynamic;
    }
    private IEnumerator EnableHitboxForSeconds(float time)
    {
        hitBox.SetActive(true);
        yield return new WaitForSeconds(time);
        hitBox.SetActive(false);
    }
}
