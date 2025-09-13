using UnityEngine;

public class SoulRenderController : MonoBehaviour
{
    [Header("Referrences")]
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private Transform player;
    [SerializeField] private BossHealthController healthController;
    [SerializeField] private Rigidbody2D rb;

    [Header("Setting")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float attackRage;
    [SerializeField] private float attackCooldown;

    [Header("Animation")]
    private int Idle_PARAM = Animator.StringToHash("Idle");
    private int Walk_PARAM = Animator.StringToHash("Walk");
    private int Attack01_PARAM = Animator.StringToHash("Attack01");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

  
    private void Move()
    {
        bossAnimator.SetTrigger(Walk_PARAM);
        Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);


        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
    }
}
