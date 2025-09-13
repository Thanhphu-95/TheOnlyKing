using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ErethonController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Rigidbody2D PlayerRb;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealthController playerHealth;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float runSpeed;
    [SerializeField] private float teleportCooldown;
    private float teleportTimer;
    [SerializeField] private float moveBehaviorCooldown = 2f; 
    private float moveBehaviorTimer = 0f;
    private int currentBehavior;


    [Header("Jump")]
    [SerializeField] private float jumpCooldown = 2f; // thời gian hồi nhảy
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpHeight = 3f;
    private float jumpTimer = 0f; // biến đếm ngược
    private bool hasJumped = false;


    [Header("Attack")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackTimer;


    private int Idle_PARAM = Animator.StringToHash("Idle");
    private int Run_PARAM = Animator.StringToHash("Run");
    private int ATTACK01_PARAM = Animator.StringToHash("Attack1");
    private int ATTACK02_PARAM = Animator.StringToHash("Attack2");
    private int JUM_PARAM = Animator.StringToHash("Jump");
    void Start()
    {
        jumpTimer = jumpCooldown;
        teleportTimer = teleportCooldown;
        attackTimer = attackCooldown;
        moveBehaviorTimer = moveBehaviorCooldown;

        playerHealth = player.GetComponent<PlayerHealthController>();
        hitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   float distance = Vector2.Distance(player.transform.position, transform.position);
        LookAtPlayer();
        teleportTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
        moveBehaviorTimer -= Time.deltaTime;



        if (distance <= attackDistance)
        {
            if (Random.value < 0.5f)
            {
                Attack02();
            }
            else
            {
                Attack01();
            }

        }
        else
        {
            float rand = Random.value;
            Debug.Log("follow player");
            if (moveBehaviorTimer <= 0)
            {

                if (rand < 0.2f && teleportTimer <= 0) //20%
                {
                    currentBehavior = 3;

                }
                else if (rand < 0.4f && jumpTimer <= 0) //20%%
                {
                    currentBehavior = 2;
                  
                }
                else if (rand < 0.6) //20%
                {
                    currentBehavior = 1;
               
                }
                else//50%
                {
                    currentBehavior = 0;
                 
                }
                moveBehaviorTimer = moveBehaviorCooldown;
            }

            switch (currentBehavior)
            {
                case 0: Move(); break;
                case 1: Run(); break;
                case 2: JumToPlayer(); break;
                case 3: TeleportBehindPlayer(); currentBehavior = 0; break;
            }


        }
    }

    private void Move()
    {
        Debug.Log("Move");
        Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance > attackDistance)
        {
            animator.SetTrigger(Idle_PARAM);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Run()
    {
        Debug.Log("Run");
        Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance > attackDistance)
        {
            animator.SetTrigger(Run_PARAM);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, runSpeed * Time.deltaTime);
        }
    }
    private void JumToPlayer()
    {
        //Vector2 direction = (player.position - transform.position).normalized;

        //float jumpDuration = 0.5f; // thời gian bay lên và xuống (giả lập)
        //float velocityX = (player.position.x - transform.position.x) / jumpDuration;

        //rb.linearVelocity = new Vector2(velocityX, jumpForce); // bật lên và di chuyển ngang
        //animator.SetTrigger(JUM_PARAM);

        //hasJumped = true;

        //rb.linearVelocity = new Vector2(PlayerRb.linearVelocityX, jumpForce);
        //animator.SetTrigger(JUM_PARAM);
        Debug.Log("Jump");
        if (hasJumped || jumpTimer > 0) return;
        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position;
        float jumpHeight = 3f; 
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float timeUp = Mathf.Sqrt(2 * jumpHeight / gravity);
        float totalTime = timeUp * 2f; 
        float velocityY = Mathf.Sqrt(2 * gravity * jumpHeight);
        float velocityX = (targetPos.x - startPos.x) / totalTime;

        rb.linearVelocity = new Vector2(velocityX, velocityY);
        animator.SetTrigger(JUM_PARAM);

        hasJumped = true;
        jumpTimer = jumpCooldown;
    }

    private void TeleportBehindPlayer()
    {
        Debug.Log("Tele");
        float offset = 1f; // khoảng cách đứng sau lưng player
        Vector3 targetPos;

        if (player.localScale.x > 0)
        {
            // Player quay phải, enemy đứng sau bên trái
            targetPos = player.position - new Vector3(offset, 0, 0);
        }
        else
        {
            // Player quay trái, enemy đứng sau bên phải
            targetPos = player.position + new Vector3(offset, 0, 0);
        }

        // Dịch chuyển tức thời
        transform.position = targetPos;

        // Optional: chơi animation teleport
        animator.SetTrigger("Teleport");
        teleportTimer = teleportCooldown;
        attackTimer = 0.3f;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            hasJumped = false;
    }


    private void Attack01()
    {
        if (attackTimer <= 0)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(hitBox.transform.position, attackRange, playerLayer);
            if (hitPlayer != null)
            {
            animator.SetTrigger(ATTACK01_PARAM);             
            Debug.Log("gây sát thường");
            StartCoroutine(Attack(0.1f));
            attackTimer = attackCooldown;
            }
        }
    }

    private void Attack02()
    {
        if (attackTimer <= 0)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(hitBox.transform.position, attackRange, playerLayer);
            if (hitPlayer != null)
            {
                animator.SetTrigger(ATTACK02_PARAM);
                Debug.Log("gây sát thường");
                StartCoroutine(Attack(0.1f));
                attackTimer = attackCooldown;

            }
        }
    }



    private IEnumerator Attack(float time)
    {
        hitBox.SetActive(true);
        playerHealth.DamagePlayer(20);
        yield return new WaitForSeconds(time);
        hitBox.SetActive(false);
    }


    private void LookAtPlayer()
    { 
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (hitBox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitBox.transform.position, attackRange);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }


}
