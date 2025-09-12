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
    [SerializeField] private GameObject teleAppearEff;
    [SerializeField] private GameObject teleDisappearEff;

    [SerializeField] private float activeTime; // thời gian boss hoạt động trước khi biến mất
    [SerializeField] private float disAppearTime; // thời gian boss biến mất
    [SerializeField] private float inActiveTime;
    [SerializeField] private float ShootTime;



    [Header("Attack 02")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private int bulletCountAttack02;
    private bool isAtAttackPoint02 = false;


    [Header("Attack 03")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform attackPoint03;
    [SerializeField] private int bulletCountAttack03;
    private int currentbullet03;
    private bool isAtAttackPoint03 = false;
    private bool hasShotAttack03 = false;
    private bool TakeDamageAttack03 = false;

    private float activeTimeCounter; // bộ đếm cho activeTime
    private float disAppearTimeCounter; // bộ đếm cho disAppearTime
    private float inActiveTimeCounter;
    private float shootCounter;


    [SerializeField] private Transform[] spawnPoints;
    private Transform targetPoint;
    [SerializeField] private Transform Theboss;
    [SerializeField] private Transform player;

    [Header("SkillCounter")]
    [SerializeField] private float skill02CountTime;
    [SerializeField] private float skill03CountTime;
    
    private float skill2Counter;
    private float skill3Counter;
    private bool enterPhase02 = true;
    private bool enterPhase03 = true;


    private int APPEAR_PARAM = Animator.StringToHash("XuatHien");
    private int Disappear_PARAM = Animator.StringToHash("BienMat");
    private int PHASE02_PARAM = Animator.StringToHash("Phase02");


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
        skill2Counter = skill02CountTime;
        skill3Counter = skill03CountTime;
        currentbullet03 = bulletCountAttack03;



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
            Phase01();
         


        }
        else if(healthController.CurrenHealth >= (healthController.MaxHealth * 0.5f) && healthController.CurrenHealth <= (healthController.MaxHealth * 0.7f)) // mau tren 50%
        {
            bossAnima.SetTrigger(PHASE02_PARAM);
            Phase02();

        }
        else 
        {
            Phase03();
        }

    }

    private void Phase01()
    {
        MoveBetweenPoints();
        Attack01();
    }

    private void Phase02()
    {

        if (enterPhase02)
        {
            if (Random.value < 0.5f)
            {
                MoveBetweenPoints();
            }
            else
            {
                TeleBetweemPoints();
            }
            Attack02();
            skill2Counter = skill02CountTime;
            enterPhase02 = false;
            return;
        }
        MoveBetweenPoints();
        Attack01();

        skill2Counter -= Time.deltaTime;
        if (skill2Counter <= 0)
        {
            if (Random.value < 0.5f)
            {
                MoveBetweenPoints();
            }
            else
            {
                TeleBetweemPoints();
            }
            Attack02();
            skill2Counter = skill02CountTime;
            return;
        }
    }
    
    private void Phase03()
    {
        if (enterPhase03)
        {
            Debug.Log("vao P3");
            skill3Counter = skill03CountTime;
            hasShotAttack03 = false;
            Attack03();

            enterPhase03 = false;
            return;
        }

        skill2Counter -= Time.deltaTime;
        if (skill2Counter <= 0 && skill3Counter > 0)
        {
            if (Random.value < 0.5f)
            {
                MoveBetweenPoints();
            }
            else
            {
                TeleBetweemPoints();
            }
            Attack02();
            skill2Counter = skill02CountTime;
            
        }
        else
        {
            MoveBetweenPoints();
            Attack01();
        }

        skill3Counter -= Time.deltaTime;
        if (skill3Counter <= 0 && skill2Counter > 0 )
        {
            Attack03();
            
            return;
        }


        //if (hasShotAttack03 && skill3Counter > 0 && skill2Counter > 0)
        //{
        //    MoveBetweenPoints();
        //    Attack01();
        //}

        if (skill3Counter <= -5 && skill2Counter <= -5)
        {
            skill3Counter = skill03CountTime;
            skill2Counter = skill02CountTime;
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

    private void TeleBetweemPoints()
    {
        StartCoroutine(TelePoint());
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
            StartCoroutine(ShootAttack02());
    }

    private void Attack03()
    {
        

        if (!isAtAttackPoint03)
        {
            
            // Boss di chuyển đến chỗ bắn
            Theboss.position = Vector3.MoveTowards(
                Theboss.position,
                attackPoint03.position,
                moveSpeed * Time.deltaTime
            );
            Debug.Log("đang đến point");

            if (Vector3.Distance(Theboss.position, attackPoint03.position) <= 0.1f)
            {Debug.Log("đã tới point");

                isAtAttackPoint03 = true;
                
            }
            return;
        }


        if (isAtAttackPoint03 = true && skill3Counter <= 0f && !hasShotAttack03)
        {
            Debug.Log("đủ ĐK");
            hasShotAttack03 = true;
            StartCoroutine(ShootAttack03());
            Debug.Log("Boss bắt đầu bắn Attack03");
            
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
        Debug.Log("vào hàm bắn");
        float[] speedOption = { 3, 3, 4, 5, 5, 6, 6, 8 }; 
        for (int i = 0; i < bulletCountAttack03; i++)
        {
            Debug.Log("bắn nè");
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
            
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(3f);
        isAtAttackPoint03 = false;
        skill3Counter = skill03CountTime;
        hasShotAttack03 = false;
        targetPoint = null;
        
    }

    private IEnumerator ShootAttack02()
    {
        float[] gravityOptions = { 1, 3, 5, 7 };


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

    private IEnumerator TelePoint()
    {
        if (teleDisappearEff != null)
        {
            Theboss.GetComponentInChildren<SpriteRenderer>().enabled = false;
            GameObject a = Instantiate(teleDisappearEff, Theboss.position, Quaternion.identity);
            Destroy(a, 1f);
        }
        

        yield return new WaitForSeconds(1f);
       
        Transform newPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int breaker = 0;
        while (newPoint == targetPoint && breaker < 50)
        {
            newPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            breaker++;
        }

        targetPoint = newPoint;

        Theboss.position = targetPoint.position;

        if (teleAppearEff != null)
        {
            GameObject b = Instantiate(teleAppearEff, Theboss.position, Quaternion.identity);
            Destroy(b, 1f);
        }
        yield return new WaitForSeconds (0.8f);
        Theboss.GetComponentInChildren<SpriteRenderer>().enabled = true;

    }

}
