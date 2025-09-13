using System.Collections;
using UnityEngine;

public class KingController : MonoBehaviour
{
    [Header("Control King")]
    [SerializeField] private Rigidbody2D kingRb;
    [SerializeField] private LayerMask layerToCheck;
    [SerializeField] private Transform groundPoin;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int currentDamage;

    private bool isOnGround;
    private bool isDoubleJump;
    private int damage;

    [Header("Animation King")]
    [SerializeField] private Animator animaKing;
    private int RunParam = Animator.StringToHash("Speed");
    private int JumpParam = Animator.StringToHash("isOnGround");
    private int Attack01KingParam = Animator.StringToHash("Attack01");
    private int Attack02KingParam = Animator.StringToHash("Attack02");
    private int Attack03KingParam = Animator.StringToHash("Attack03");

    [Header("Attack")]
    [SerializeField] private AttackHitbox attackHitBox;

    [Header("Dash")]
    [SerializeField] private SpriteRenderer kingSR;
    [SerializeField] private SpriteRenderer dashEffectSR;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashEffectLifeTime;
    [SerializeField] private float timeBetweenEachDashEffect;
    [SerializeField] private float dashCoolDownTime;

    private float dashCounter;
    private float dashCoolDownCounter;
    private float dashEffectCounter;
    private bool isDash;
    public bool IsDash => isDash;

    private void Update()
    {
        Run();
        Jump();
        Attack01();
        Attack02();
        Attack03();
        Dash();
        UpdateAnimation();
    }

    private void Run()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        kingRb.linearVelocity = new Vector2(xAxis * moveSpeed, kingRb.linearVelocityY);

        if (kingRb.linearVelocityX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (kingRb.linearVelocityX > 0)
            transform.localScale = Vector3.one;
    }

    private void Jump()
    {
        isOnGround = Physics2D.OverlapCircle(groundPoin.position, 0.2f, layerToCheck);

        if (Input.GetButtonDown("Jump") && (isOnGround || isDoubleJump))
        {
            if (isOnGround)
            {
                kingRb.linearVelocity = new Vector2(kingRb.linearVelocityX, jumpForce);
                isDoubleJump = true;
            }    
                
            else
            {
                kingRb.linearVelocity = new Vector2(kingRb.linearVelocityX, jumpForce);
                animaKing.SetTrigger(JumpParam);
                isDoubleJump = false;
            }

            
        }
    }

    private void Attack01()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animaKing.SetTrigger(Attack01KingParam);
            attackHitBox.SetDamage(30);
            attackHitBox.DoAttack();
        }
    }

    private void Attack02()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            animaKing.SetTrigger(Attack02KingParam);
            attackHitBox.SetDamage(40);
            attackHitBox.DoAttack();
        }
    }

    private void Attack03()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animaKing.SetTrigger(Attack03KingParam);
            attackHitBox.SetDamage(50);
            attackHitBox.DoAttack();
        }
    }

    private void Dash()
    {
        if (dashCoolDownCounter > 0)
        {
            dashCoolDownCounter -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashCounter = dashTime;
            ShowDashEffect();
            isDash = true;
            animaKing.SetTrigger("Dash");
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            kingRb.linearVelocity = new Vector2(dashSpeed * transform.localScale.x, 0f);
            dashEffectCounter -= Time.deltaTime;

            if (dashEffectCounter <= 0)
                ShowDashEffect();

            if (dashCounter <= 0)
            {
                dashCoolDownCounter = dashCoolDownTime;
                isDash = false;
            }
        }
    }

    private void ShowDashEffect()
    {
        SpriteRenderer spriteRenderer = Instantiate(dashEffectSR, transform.position, transform.rotation);
        spriteRenderer.sprite = kingSR.sprite;
        spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, -35f);
        spriteRenderer.transform.localScale = transform.localScale;

        Destroy(spriteRenderer.gameObject, dashEffectLifeTime);
        dashEffectCounter = timeBetweenEachDashEffect;
    }

    private void UpdateAnimation()
    {
        float speeX = Mathf.Abs(kingRb.linearVelocityX);
        animaKing.SetFloat(RunParam, speeX);
        animaKing.SetBool(JumpParam, isOnGround);
    }

    public void IncreaseDamage(int dmg)
    {
        damage += dmg;
        currentDamage = damage;
    }
}


