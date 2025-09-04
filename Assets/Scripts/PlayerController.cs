using System.Collections;
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
    [SerializeField] private int currentDamage;
    private bool isOnGround;
    private bool isDoubleJump;
    private int damage;

    [SerializeField] private Animator animationPlayer;

    [SerializeField] private AttackHitbox attackHitBox;

    [Header("Attack 02")]
    [SerializeField] private float holdAttack02;
    private float holdTimeAttack02;
    private bool isHolding;
    private bool canMove = true;


    [Header("Biến hình")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject priest;

   


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





    [Header("Animation Player")]
    private int speedParam = Animator.StringToHash("speed");
    private int isOnGroundParam = Animator.StringToHash("isOnGround");
    private int isDoubleJumpParam = Animator.StringToHash("DoubleJump");
    private int AttackParam = Animator.StringToHash("Attack");
    private int Attack02Param = Animator.StringToHash("Attack 2");
    private int DashParam = Animator.StringToHash("Dash");
    private int HoldParam = Animator.StringToHash("Hold");

    [Header("contro Priest")]
    [SerializeField] private Animator animaPriest;

    [Header("Animation Priest")]
    private int speebParamPr = Animator.StringToHash("speedpr");
    private int isDoubleJumpParamPr = Animator.StringToHash("DoubleJumpPr");
    private int isOnGroundParamPr = Animator.StringToHash("isOnGroundPr");  
    private int AttackParamPr = Animator.StringToHash("attack01Pr");
    private int Attack02ParamPr = Animator.StringToHash("attack02Pr");
    private int Attack03ParamPr = Animator.StringToHash("attack03Pr");
    




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
        
        AnimationPlayer();
        AnimaPriest();
        Attack01();
        Attack02();
        Attack03();
        Dash();
        Priest();
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
                if (player.activeSelf)
                {
                    animationPlayer.SetTrigger(isDoubleJumpParam);
                }
                else if (priest.activeSelf)
                {
                    Debug.Log("Jump");
                    animaPriest.SetTrigger(isDoubleJumpParamPr);
                }

                isDoubleJump = false;
            }
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocityX, jumpForce);
        }
        
    }

    private void Attack01()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (player.activeSelf)
            {
                animationPlayer.SetTrigger(AttackParam);
                attackHitBox.SetDamage(10);
            }
            else if (priest.activeSelf)
            {
                animaPriest.SetTrigger(AttackParamPr);
                attackHitBox.SetDamage(15);
            }
          
        }

    }

    private void Attack02()
    {
        if (player.activeSelf)
        {
             if (Input.GetButtonDown("Fire2") && isOnGround)
             {
                    playerRb.linearVelocity = new Vector2(0, playerRb.linearVelocityY);
                    holdTimeAttack02 = 0f;
                    isHolding = true;
                    GetComponent<PlayerController>().canMove = false;
                    Debug.Log("Bắt đầu giữ Fire2 -> HoldParam ON");
                    animationPlayer.SetBool(HoldParam, isHolding);
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
                animationPlayer.SetBool(HoldParam, false);
                animationPlayer.SetTrigger(Attack02Param);
                    StartCoroutine(MoveForward(10f, 0.3f));
                }
                else
                {
                Debug.Log("ko đủ thời gian -> hủy attack");
                animationPlayer.SetBool(HoldParam, false);

                }
            
                isHolding = false;
                GetComponent<PlayerController>().canMove = true;
            }
        }
        else if (priest.activeSelf && isOnGround)
        {
            if (Input.GetButtonDown("Fire2"))
            {               
                animaPriest.SetTrigger(Attack02ParamPr);          
            }
        }

    }

    private void Attack03()
    {
        if (priest.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Q)) 
            {
                animaPriest.SetTrigger(Attack03ParamPr); 
            }
        }
        
    }

    private void Priest()
    {
        if (priest.activeSelf == false)
        {
            Debug.Log("Ko phải Priest");
            if ( Input.GetKeyDown(KeyCode.Alpha2))
            {
                player.SetActive(false);
                priest.SetActive(true);
            }
            
        }
        else if (!player.activeSelf && priest.activeSelf)
        {
              if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player.SetActive(true);
                priest.SetActive(false);
            }
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
                animationPlayer.SetTrigger(DashParam);
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

    private IEnumerator MoveForward(float speed, float duration)
    {
        float timer = duration;
        while (timer > 0)
        {   
            timer -= Time.deltaTime;
            playerRb.linearVelocity = new Vector2(speed * transform.localScale.x, playerRb.linearVelocityY);
            
            yield return null;
        }
        playerRb.linearVelocity = new Vector2(0f, playerRb.linearVelocityY);
    }


    public void IncreaseDamage(int dmg)
    {
        damage = damage + dmg;
        currentDamage = damage;
    }

    private void AnimationPlayer()
    {
        float speeX = Mathf.Abs(playerRb.linearVelocityX);
        animationPlayer.SetFloat(speedParam, speeX);
        animationPlayer.SetBool(isOnGroundParam, isOnGround);
        animationPlayer.SetBool(HoldParam, isHolding);
    }

    private void AnimaPriest()
    {
        float speeX = Mathf.Abs(playerRb.linearVelocityX);
        animaPriest.SetFloat(speebParamPr, speeX);
        animaPriest.SetBool(isOnGroundParamPr, isOnGround);

    }
}

