using UnityEngine;

public class GhostBossController : MonoBehaviour
{

    [SerializeField] private Animator bossAnima;
    [SerializeField] private BossHealthController healthController;
    [SerializeField] private GameObject bossBullerEff;
    [SerializeField] private GameObject fireBallEff;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float activeTime; // thời gian boss hoạt động trước khi biến mất
    [SerializeField] private float disAppearTime; // thời gian boss biến mất
    [SerializeField] private float inActiveTime;
    [SerializeField] private float ShootTime;

    [Header("Attack 02")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private int bulletCountAttack02;
    [SerializeField] private float timeToAttack02;
    private bool isAtAttackPoint = false;
    private float attack02Counter;
    private float activeTimeCounter; // bộ đếm cho activeTime
    private float disAppearTimeCounter; // bộ đếm cho disAppearTime
    private float inActiveTimeCounter;
    private float shootCounter;

    private bool Phase01;
    private bool Phase02;


    
    


    [SerializeField] private Transform[] spawnPoints;
    private Transform targetPoint;
    [SerializeField] private Transform Theboss;
    [SerializeField] private Transform player;

    private void Awake()
    {
        foreach (var item in spawnPoints)
        {
            item.SetParent(null);
        }
    }

    void Start()
    {
        healthController = GetComponent<BossHealthController>();
        activeTimeCounter = activeTime;
        shootCounter = ShootTime;

        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        Debug.Log($"mau hien tai{healthController.CurrenHealth}");
        if (  healthController.CurrenHealth >= (healthController.MaxHealth / 2)) //mau boss tren 50%
        {
            
            Debug.Log("Phase 01");
            
            Attack02();
            Phase01 = true;
            Phase02 = false;
        }
        else
        {
            Debug.Log("Phase 02");
            Phase01 = false;
            Phase02 = true;
        }

    }

   private void MoveBetweenPoints()
    {
        if ( targetPoint == null)
        {
            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        Theboss.position = Vector3.MoveTowards(Theboss.position, targetPoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(Theboss.position, targetPoint.position) <= 0.1f)
        {
            Transform newPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            int whileBreaker = 0;

            while (newPoint.position == Theboss.position && whileBreaker <= 50)
            {
                newPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                whileBreaker++;
            }
            if (whileBreaker == 50)
            {
                whileBreaker = 0;
            }
            targetPoint = newPoint;
        }
   }


    private void Attack01()
    {
        shootCounter -= Time.deltaTime;
        if (shootCounter < 0)
        {

            GameObject bullet = Instantiate(bossBullerEff, shootPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 dir = (player.position - shootPoint.position).normalized;

            
            rb.linearVelocity = dir * 10f;

            shootCounter = ShootTime;
        }
    }

    private void Attack02()
    {
       
        if (!isAtAttackPoint) 
        {
            Theboss.position = Vector3.MoveTowards(Theboss.position, attackPoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(Theboss.position, attackPoint.position) <= 0.1f)
            {
                isAtAttackPoint = true;
                attack02Counter = timeToAttack02;
            }
            return;
        }

        attack02Counter -= Time.deltaTime;

        if (attack02Counter <= 0)
        {
            for (int i = 0; i < bulletCountAttack02; i++ )
            {
                GameObject bullet = Instantiate(fireBallEff, shootPoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                Vector2 dir = (player.position - shootPoint.position).normalized;

                float randomSpeed = Random.Range(10f, 30f);
                rb.linearVelocity = dir * randomSpeed;
                rb.gravityScale = 1f;   
            }
            attack02Counter = timeToAttack02;
            isAtAttackPoint = false;
        }


    }

    private void LookAtPlayer()
    {
        if (player.position.x > Theboss.position.x)
        {
            Theboss.localScale = Vector3.one;
        }
        else
        {
            Theboss.localScale = new Vector3(-1, 1, 1);
        }


    }

}
