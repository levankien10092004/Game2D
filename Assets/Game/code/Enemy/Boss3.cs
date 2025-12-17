using UnityEngine;

public class Boss3 : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] bool isRight = true;
    [SerializeField] float chaseSpeed = 3f;
    Rigidbody2D rb;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] float attackRange = 4f;
    [SerializeField] float attackDistance = 1.6f;
    bool inRange;

    // ================= MÁU (LẤY TỪ BOSS2) =================
    [Header("Máu")]
    public float health = 100;
    public float healthnow;
    public HPEnemy quai;
    bool isDead = false;

    [Header("Attack")]
    [SerializeField] Transform attackPoint;
    public float attackDamage = 10f;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    [SerializeField] float attackCooldown = 1.5f;
    float lastAttackTime;
    bool isAttacking;

    [Header("Skill 2 - Shoot")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Animation")]
    public Animator animator;

    [Header("Rơi đồ")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 5;
    public float healDropRate = 0.2f;
    public GameObject victory;

    enum SkillState { None, ChaseSlash, Shoot }
    SkillState currentSkill = SkillState.None;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (quai == null)
            quai = GetComponentInChildren<HPEnemy>();

        MauKhoiTao();
    }

    void Update()
    {
        if (isDead) return;

        CheckRange();
        HandleAI();
    }

    // ================= KHỞI TẠO MÁU =================
    void MauKhoiTao()
    {
        healthnow = health;
        if (quai != null)
            quai.capNhatMau(healthnow, health);
    }

    // ================= AI =================
    void HandleAI()
    {
        if (!inRange || isAttacking)
        {
            animator.SetBool("Run", false);
            return;
        }

        FacePlayer();
        float dist = Vector2.Distance(transform.position, player.position);

        if (currentSkill != SkillState.None)
        {
            if (currentSkill == SkillState.ChaseSlash)
                HandleChaseSlash(dist);
            return;
        }

        if (Time.time - lastAttackTime < attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (dist <= attackDistance)
        {
            currentSkill = SkillState.ChaseSlash;
        }
        else
        {
            currentSkill = (Random.value < 0.7f)
                ? SkillState.Shoot
                : SkillState.ChaseSlash;

            if (currentSkill == SkillState.Shoot)
            {
                isAttacking = true;
                animator.SetTrigger("Attack2");
            }
        }
    }

    // ================= SKILL 1 =================
    void HandleChaseSlash(float dist)
    {
        if (dist > attackDistance)
        {
            animator.SetBool("Run", true);
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );
        }
        else
        {
            animator.SetBool("Run", false);
            rb.velocity = Vector2.zero;

            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack1");
            }
        }
    }

    // ================= ATTACK EVENT =================
    void attack()
    {
        Vector2 dir = isRight ? Vector2.right : Vector2.left;
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRadius,
            attackLayer
        );

        foreach (Collider2D c in hits)
        {
            Vector2 toTarget = (c.transform.position - transform.position).normalized;
            if (Vector2.Angle(dir, toTarget) < 45f)
            {
                HeroKnight hero = c.GetComponent<HeroKnight>();
                if (hero) hero.TakeDamage(attackDamage);
            }
        }
    }

    // ================= SHOOT EVENT =================
    void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BulletEnemy bullet = b.GetComponent<BulletEnemy>();

        // bắn vào TÂM HITBOX player
        Collider2D col = player.GetComponentInChildren<Collider2D>();
        bullet.SetTarget(col.bounds.center);
    }

    // ================= END ATTACK =================
    public void EndAttack()
    {
        isAttacking = false;
        currentSkill = SkillState.None;
    }

    // ================= MÁU & CHẾT (BOSS2 STYLE) =================
    public void TakeDamage(int damPlayer)
    {
        if (isDead) return;

        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);

        if (healthnow <= 0)
            animator.SetTrigger("Dead");
    }

    void Die()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < IntCoin; i++)
        {
            SpawnObject(coinPrefab);
        }

        if (Random.value <= healDropRate)
            SpawnObject(healPrefab);

        victory.gameObject.SetActive(true);
    }

    // ================= SUPPORT =================
    void FacePlayer()
    {
        if (player.position.x > transform.position.x && !isRight) Flip();
        else if (player.position.x < transform.position.x && isRight) Flip();
    }

    void Flip()
    {
        isRight = !isRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    void CheckRange()
    {
        inRange = Vector2.Distance(transform.position, player.position) <= attackRange;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
