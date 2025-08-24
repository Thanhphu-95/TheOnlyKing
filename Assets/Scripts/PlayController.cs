using UnityEngine;


public class PlayController : MonoBehaviour
{
    [Header("contro nhân vật")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private LayerMask layerToCheck;
    [SerializeField] private Transform groundPoin;
    [SerializeField] private float moveSpeed; // tốc độ di chuyển
    [SerializeField] private float jumpForce;// tôc độ nhảy
    private bool isOnGround;
    private bool isDoubleJump;
    [SerializeField] private Animator animationHero;


    public int maxHealth = 200;
    public int currentHealth;
    [SerializeField] private HealthEnemi health;




    [Header("Animation")]
    private int speedParam = Animator.StringToHash("speed");
    private int isOnGroundParam = Animator.StringToHash("isOnGround");
    private int isDoubleJumpParam = Animator.StringToHash("DoubleJump");
    private int AttackParam = Animator.StringToHash("Attack");


    private void Start()
    {
        currentHealth = maxHealth;
        health.SetMaxHealth(maxHealth);
    }
    void Update()
    {
        
        Run();
        Jump();
        Animation();
        Attack();
        TakeDamege();
    }

    private void Run()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        playerRb.linearVelocity = new Vector2( xAxis * moveSpeed, playerRb.linearVelocityY );
        if (playerRb.linearVelocityX<0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerRb.linearVelocityX>0)
        {
            transform.localScale = Vector3.one;
        }
    }    

    private void Jump()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoin.position, 0.2f, layerToCheck);
        Debug.Log($"is On Ground{isOnGround}");

        if (Input.GetButtonDown("Jump") && (isOnGround || isDoubleJump))
        {
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    animationHero.SetTrigger(AttackParam);
            //}

            if (isOnGround)
            {
                isDoubleJump = true;
            }
            else 
            {
                animationHero.SetTrigger(isDoubleJumpParam);
                isDoubleJump = false;
            }
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocityX, jumpForce);
        }
    }    

    private void Attack()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            animationHero.SetTrigger(AttackParam);
        }    
    }    


    private void Animation()
    {
        float speeX = Mathf.Abs(playerRb.linearVelocityX);
        animationHero.SetFloat(speedParam, speeX);
        animationHero.SetBool(isOnGroundParam, isOnGround);


    }    




    private void TakeDamege()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth = currentHealth - 20;
            health.SetHealth(currentHealth);
        }
    }    
}

//-2.607703e-06  -1.190303