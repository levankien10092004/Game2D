using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] protected bool isRight = true;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float moveDistance = 4f;

    [Header("Trạng thái")]
    public float health = 50;
    public float healthnow;
    public HPEnemy quai;
    protected float chaseSpeed = 3f;
    private bool isAttacking = false;
    private bool isReturning = false;
    private bool isDead = false;

    [SerializeField] protected float attackDistance = 5f;

    protected Vector2 startPos;
    protected Rigidbody2D rb;
    protected Vector3 attackStartPos;

    [SerializeField] protected Transform Layer;
    [SerializeField] protected bool isRange = false;
    [SerializeField] protected float detectRange = 10f;

    float offsetY = 1.5f;

    [SerializeField] protected Animator animator;

    [Header("Cooldown & Damage")]
    [SerializeField] protected float attackCooldown = 2f;
    protected float lastAttackTime;
    [SerializeField] public float collisionDamage = 20f;
    [SerializeField] public float pounceForce = 8f;
    [SerializeField] LayerMask Map;
    private bool isGrounded;

    [Header("Bắn đạn")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private bool isShooting = false;

    [Header("Rơi đồ")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 5;
    public float healDropRate = 0.2f;
    public GameObject victory;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        if (quai == null) quai = GetComponentInChildren<HPEnemy>();
        if (quai != null) mau();

        if (animator == null) animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        CheckDetectRange();

        if (!isAttacking && !isShooting)
            Move();

        // ----- QUYẾT ĐỊNH TẤN CÔNG -----
        if (isRange && !isAttacking && !isReturning)
        {
            float currentTime = Time.time;

            if (currentTime > lastAttackTime + attackCooldown)
            {
                float dist = Vector2.Distance(transform.position, Layer.position);

                // ★ Ưu tiên tấn công bổ nhào nếu player ở gần
                if (dist <= attackDistance)
                {
                    animator.SetBool("Attack1", true);
                    animator.SetBool("Attack2", false);

                    StartCoroutine(Attack());
                    lastAttackTime = currentTime;
                    return;
                }

                // ★ Player ở xa → BẮN
                animator.SetBool("Attack2", true);
                animator.SetBool("Attack1", false);

                Shoot();
                lastAttackTime = currentTime;
                return;
            }
        }

        CheckGround();
    }

    public void mau()
    {
        healthnow = health;
        if (quai != null) quai.capNhatMau(healthnow, health);
    }

    protected virtual void Move()
    {
        // ▬▬▬ 1. QUAY VỀ SAU TẤN CÔNG ▬▬▬
        if (isReturning)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackStartPos, chaseSpeed * Time.deltaTime);

            // Về tới vị trí ban đầu
            if (Vector3.Distance(transform.position, attackStartPos) < 0.1f)
            {
                isReturning = false;
            }
            return;
        }

        // ▬▬▬ 2. ĐUỔI THEO ▬▬▬
        if (isRange)
        {
            // Xoay mặt theo player
            if ((Layer.position.x > transform.position.x && !isRight) ||
                (Layer.position.x < transform.position.x && isRight))
            {
                Flip();
            }

            Vector3 targetPos = new Vector3(Layer.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        }
        else
        {
            // ▬▬▬ 3. TUẦN TRA ▬▬▬
            Vector2 dir = isRight ? Vector2.right : Vector2.left;
            transform.Translate(dir * speed * Time.deltaTime);

            if (isRight && transform.position.x >= startPos.x + moveDistance)
                Flip();
            else if (!isRight && transform.position.x <= startPos.x - moveDistance)
                Flip();
        }
    }

    // ▬▬▬ BỔ NHÀO ▬▬▬
    protected IEnumerator Attack()
    {
        attackStartPos = transform.position;
        isAttacking = true;
        rb.velocity = Vector2.zero;

        Vector2 direction = (Layer.position - transform.position).normalized;
        rb.velocity = direction * pounceForce;

        yield return new WaitForSeconds(0.8f);

        rb.velocity = Vector2.zero;
        isAttacking = false;

        animator.SetBool("Attack1", false);

        isReturning = true;
    }

    void Shoot()
    {
        isShooting = true;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
   //     bullet.transform.rotation = Quaternion.Euler(0, 0, -90);

        bullet.GetComponent<BulletEnemy>().SetTarget(Layer.position);

        StartCoroutine(StopShoot());
    }

    IEnumerator StopShoot()
    {
        yield return new WaitForSeconds(1.5f);
        isShooting = false;
        animator.SetBool("Attack2", false);
    }
     
    protected void Flip() 
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale; 
    }  
      
    protected void CheckDetectRange()
    {
        isRange = (Vector2.Distance(transform.position, Layer.position) <= detectRange);
    }

    public void TakeDamage(int damPlayer)
    {
        if (isDead) return;

        healthnow -= damPlayer;
        if (quai != null) quai.capNhatMau(healthnow, health);

        if (healthnow <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && isAttacking)
        {
            rb.velocity = Vector2.zero;

            HeroKnight hero = Layer.GetComponent<HeroKnight>();
            if (hero != null)
                hero.TakeDamage((int)collisionDamage);

            isAttacking = false;
            isReturning = true;

            animator.SetBool("Attack1", false);
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, Map);

        if (isGrounded)
        {
            isAttacking = false;
            isReturning = true;
            rb.velocity = Vector2.zero;
            animator.SetBool("Attack1", false);
        }
    }

    protected void Die()
    {
        gameObject.SetActive(false);
        for(int i = 0; i < IntCoin; i++)
        {
            SpawnObject(coinPrefab);
        }
        
        if (Random.value <= healDropRate)
            SpawnObject(healPrefab);

        victory.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    void SpawnObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        Rigidbody2D rb2 = obj.GetComponent<Rigidbody2D>();
        if (rb2 != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb2.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }
}
