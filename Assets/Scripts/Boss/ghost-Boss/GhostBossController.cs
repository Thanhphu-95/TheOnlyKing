using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private bool isAtAttackPoint02 = false;
    private float attack02Counter;
    


    [Header("Attack 03")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform attackPoint03;
    [SerializeField] private int bulletCountAttack03;
    [SerializeField] private float timeToAttack03;
    private float attack03Counter;
    private bool isAtAttackPoint03 = false;
    private bool TakeDamageAttack03 = false;


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
        attack03Counter = timeToAttack03;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        LookAtPlayer();
        Debug.Log($"mau hien tai{healthController.CurrenHealth}");
        if (  healthController.CurrenHealth > (healthController.MaxHealth * 0.7f)) //mau boss tren 80%
        {
            
            Debug.Log("Phase 01");
            //MoveBetweenPoints();
            //Attack02();
            //if (attack03Counter <= 0f)
            //{
            //    Attack03();
            //    attack03Counter = timeToAttack03; // reset thời gian chờ giữa mỗi lần tấn công
            //}
            //else
            //{
            //    attack03Counter -= Time.deltaTime;
            //}

            Attack03();
            Phase01 = true;
            Phase02 = false;
        }
        else if(healthController.CurrenHealth >= (healthController.MaxHealth * 0.5f) && healthController.CurrenHealth <= (healthController.MaxHealth * 0.7f)) // mau tren 50%
        {   /*Attack02();*/
            Debug.Log("Phase 02");
            Phase01 = false;
            Phase02 = true;
        }
        else 
        {
            MoveBetweenPoints();
            Attack01();
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
        if (!isAtAttackPoint02) 
        {
            Theboss.position = Vector3.MoveTowards(Theboss.position, attackPoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(Theboss.position, attackPoint.position) <= 0.1f)
            {
                isAtAttackPoint02 = true;
                attack02Counter = timeToAttack02;
            }
            return;
        }
        attack02Counter -= Time.deltaTime;

        if (attack02Counter <= 0)
        {
            StartCoroutine(ShootAttack02());
            attack02Counter = timeToAttack02;
        }
    }

    private void Attack03()
    {
        bool first = true;
        if (!isAtAttackPoint03)
        {
            Theboss.position = Vector3.MoveTowards(Theboss.position, attackPoint03.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(Theboss.position, attackPoint03.position) <= 0.1f)
            {
                isAtAttackPoint03 = true;
                attack03Counter = timeToAttack03;
            }
            return;
        }
        if (attack03Counter > 0)
        {
            attack03Counter -= Time.deltaTime;
            return;
        }

        
        BossHealthController bossHealth = GetComponent<BossHealthController>();
        if (bossHealth != null) 
        {
            bossHealth.TakeDamage(10);
        }
        if (first == true)
        {
            StartCoroutine(ShootAttack03());
            first = false;
        }
        if (attack03Counter <= 0 )
        {
            StartCoroutine(ShootAttack03());
            isAtAttackPoint03 = false;
            
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

    private IEnumerator ShootAttack03()
    {
        float[] speedOption = { 3, 3, 4, 5, 5, 6, 6, 8 }; 
        for (int i = 0; i < bulletCountAttack03; i++)
        {
            GameObject fire = Instantiate(fireBallEff, shootPoint.position, Quaternion.identity);
            Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float dirX = Mathf.Sign(Theboss.localScale.x);
                Vector2 shootDir = new Vector2(Random.Range(0.1f, 0.5f) * dirX, 0.3f).normalized;
                float shootSpeed = speedOption[Random.Range(0, speedOption.Length)];
                rb.linearVelocity = shootDir * shootSpeed;

                rb.gravityScale = Random.Range(0.1f, 0.3f);
            }



            BossBullet bossBullet = fire.GetComponent<BossBullet>();
            if (bossBullet != null)
            {
                bossBullet.attack03 = true;
                bossBullet.spawnEnemy = Random.value < 0.2f;
                Debug.Log($"SpawnEnemy cho viên đạn này: {bossBullet.spawnEnemy}");
            }
            
            yield return new WaitForSeconds(0.7f);
        }
        attack03Counter = timeToAttack03;
    }

    private IEnumerator ShootAttack02()
    {
        float[] gravityOptions = { 0.1f, 1, 10, 5 };


            for (int i = 0; i < bulletCountAttack02; i++ )
            {
                GameObject bullet = Instantiate(fireBallEff, shootPoint.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                Vector2 dir = (player.position - shootPoint.position).normalized;

                float randomSpeed = Random.Range(10f, 100f);

                float randomGravity = gravityOptions[Random.Range(0, gravityOptions.Length)];
                rb.linearVelocity = dir * randomSpeed;

                rb.gravityScale = randomGravity;

            yield return new WaitForSeconds(0.5f);
            }
    }


}
