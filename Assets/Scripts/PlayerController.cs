using UnityEngine;
using UnityEngine.Rendering;


public class PlayerController : MonoBehaviour
{
    //private int playerLayer ;
    //private int enemyLayer;

    [Header("contro nhân vật")]
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private LayerMask layerToCheck;
    [SerializeField] private Transform groundPoin;
    [SerializeField] private float moveSpeed; // tốc độ di chuyển
    [SerializeField] private float jumpForce;// tôc độ nhảy
    private bool isOnGround;
    private bool isDoubleJump;
    [SerializeField] private Animator animationHero;

    [SerializeField] private HealthEnemi health;

    [Header("Attack 02")]
    [SerializeField] private float holdAttack02;
    private float holdTimeAttack02;
    private bool isHolding;
    private bool canMove = true;


    [Header("Dash")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer playerDashEffectSR;
    [SerializeField] private float dashSpeed; // tốc độ dash
    [SerializeField] private float dashTime; // thoi gian dash
    [SerializeField] private float dashEffectLifeTime;// thời gian tồn tại hiệu ứng
    [SerializeField] private float timeBetweenEachDashEffect; //khoãn cách giứa cac hiệu ứng
    [SerializeField] private float dashCoolDownTime; // thoi gian hoi chieu
    [SerializeField] private float dashEffectCounter;

    private float dashCounter; // luu time dash, dung de dem nguoc
    private float dashCoolDownCounter; // luu time hoi chieu, dung de dem nguoc
    private bool isDash;
    public bool IsDash => isDash;





    [Header("Animation")]
    private int speedParam = Animator.StringToHash("speed");
    private int isOnGroundParam = Animator.StringToHash("isOnGround");
    private int isDoubleJumpParam = Animator.StringToHash("DoubleJump");
    private int AttackParam = Animator.StringToHash("Attack");
    private int Attack02Param = Animator.StringToHash("Attack 2");
    private int DashParam = Animator.StringToHash("Dash");
    private int HoldParam = Animator.StringToHash("Hold");


    private void Start()
    {
        //playerLayer = gameObject.layer;
        //enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    void Update()
    {
        if (canMove)
        {
        Run();
        Jump();
        }
        
        Animation();
        Attack01();
        Attack02();
        Dash();
        //TakeDamege();
    }

    private void Run()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        playerRb.linearVelocity = new Vector2(xAxis * moveSpeed, playerRb.linearVelocityY);
        if (playerRb.linearVelocityX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (playerRb.linearVelocityX > 0)
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

    private void Attack01()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animationHero.SetTrigger(AttackParam);
        }
    }

    private void Attack02()
    {
        if (Input.GetButtonDown("Fire2") && isOnGround)
        {
            playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocityY);
            holdTimeAttack02 = 0f;
            isHolding = true;
            GetComponent<PlayerController>().canMove = false;
            Debug.Log("Bắt đầu giữ Fire2 -> HoldParam ON");
            animationHero.SetBool(HoldParam, isHolding);
        }    


        if(isHolding && Input.GetButton("Fire2"))
        {
            holdTimeAttack02 += Time.deltaTime; 
        }

        if (isHolding && Input.GetButtonUp("Fire2"))
        {
            if (holdTimeAttack02 >= holdAttack02)
            {
                Debug.Log("Giữ đủ thời gian -> thực hiện Attack logic!"); 
                animationHero.SetBool(HoldParam, false);
                animationHero.SetTrigger(Attack02Param);
            }
            else
            {
                Debug.Log("ko đủ thời gian -> hủy attack");
                animationHero.SetBool(HoldParam, false);

            }
            
            isHolding = false;
            GetComponent<PlayerController>().canMove = true;
        }
    }

    private void Dash()
    {
        if (dashCoolDownCounter > 0) // dang trong thoi gian hoi chieu
        {
            dashCoolDownCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashCounter = dashTime;
                ShowDashEffect();
                isDash = true;
                animationHero.SetTrigger(DashParam);
            }
        }
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            playerRb.linearVelocity = new Vector2(dashSpeed * transform.localScale.x, 0f);
            dashEffectCounter -= Time.deltaTime;


            if (dashEffectCounter <= 0)
            {
                ShowDashEffect();
            }

            if (dashCounter <= 0)
            {
                dashCoolDownCounter = dashCoolDownTime;
                isDash = false;
                //Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
            }
            
        }
    }

    private void ShowDashEffect()
    {
        SpriteRenderer spriteRenderer;
        spriteRenderer = Instantiate(playerDashEffectSR, transform.position, transform.rotation);
        
        spriteRenderer.sprite = playerSR.sprite;
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -35f);
        spriteRenderer.transform.localScale = transform.localScale;
        
        Destroy(spriteRenderer.gameObject, dashEffectLifeTime );

        dashEffectCounter = timeBetweenEachDashEffect;
    }

    private void Animation()
    {
        float speeX = Mathf.Abs(playerRb.linearVelocityX);
        animationHero.SetFloat(speedParam, speeX);
        animationHero.SetBool(isOnGroundParam, isOnGround);
        animationHero.SetBool(HoldParam, isHolding);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }


    //private void TakeDamege()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        currentHealth = currentHealth - 20;
    //        health.SetHealth(currentHealth);
    //    }
    //}    
}

//-2.607703e-06  -1.190303