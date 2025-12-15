using System.Collections;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] protected bool isRight = true;
    [SerializeField] protected float speed = 2f;

    protected Vector2 startPos;
    protected Rigidbody2D rb;

    [Header("Player")]
    [SerializeField] protected Transform Layer;
  [SerializeField]  protected bool isRange = false;
    [SerializeField] protected float atackRanger = 4f;
    [SerializeField] protected float attackDistan = 1.6f;
    protected float chaseSpeed = 3f;

    [Header("Máu")]
    public int health = 100;
    public int healthnow;
    public HPEnemy quai;
    private int intDam = 0;

    [Header("Tấn công")]
    [SerializeField] protected Transform attackPoin;
    public float attackDam = 5f;
    public float attackRadius = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;
    [SerializeField] private bool isAttacking = false;
    public LayerMask attackLayer;

    [Header("Chiêu 2 - Bắn đạn")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Animation")]
    [SerializeField] protected Animator animator;

    [Header("Rơi đồ")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 10;
    public float healDropRate = 0.2f;

    [Header("Victory")]
    public GameObject Poinend;

    enum SkillState
    {
        None,
        Attack1,
        Attack2
    }

     SkillState currentSkill = SkillState.None;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        if (quai == null)
            quai = GetComponentInChildren<HPEnemy>();

        healthnow = health;
        if (quai != null)
            quai.capNhatMau(healthnow, health);
    }

    void Update()
    {
        CheckAttackRange();
        Move();
    }

    // ===================== DI CHUYỂN & TẤN CÔNG =====================
    protected virtual void Move()
    {
        if (!isRange)
        {
            animator.SetBool("Run", false);
            return;
        }

        // quay mặt về player
        if (Layer.position.x > transform.position.x && !isRight)
            Flip();
        else if (Layer.position.x < transform.position.x && isRight)
            Flip();

        float distance = Vector2.Distance(transform.position, Layer.position);

        // ===== ĐANG ĐÁNH → KHÔNG CHỌN CHIÊU MỚI =====
        if (currentSkill != SkillState.None)
        {
            if (currentSkill == SkillState.Attack1)
            {
                // CHẠY TỚI KHI ĐỦ GẦN
                if (distance > attackDistan)
                {
                    animator.SetBool("Run", true);
                    transform.position = Vector2.MoveTowards(
                        transform.position,
                        Layer.position,
                        chaseSpeed * Time.deltaTime
                    );
                }
                else
                {
                    animator.SetBool("Run", false);
                    rb.velocity = Vector2.zero;
                }
            }
            else if (currentSkill == SkillState.Attack2)
            {
                animator.SetBool("Run", false);
                rb.velocity = Vector2.zero;
            }

            return;
        }

        // ===== CHƯA ĐÁNH → CHỌN CHIÊU =====
        if (Time.time - lastAttackTime < attackCooldown)
        {
            animator.SetBool("idea", distance > attackDistan);
            return;
        }

        lastAttackTime = Time.time;

        // ===== TRONG TẦM GẦN → CHỈ CHÉM =====
        if (distance <= attackDistan)
        {
            currentSkill = SkillState.Attack1;
            isAttacking = true;
            animator.SetTrigger("Attack1");
        }
        // ===== XA → RANDOM, ƯU TIÊN BẮN =====
        else
        {
            currentSkill = (Random.value < 0.7f)
                ? SkillState.Attack2
                : SkillState.Attack1;

            isAttacking = true;

            if (currentSkill == SkillState.Attack1)
                animator.SetTrigger("Attack1");
            else
                animator.SetTrigger("Attack2");
        }
    }

    // ===================== CHIÊU 1 – CHÉM =====================
    protected void attack()
    {
        Vector2 attackDir = isRight ? Vector2.right : Vector2.left;
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayer);

        foreach (Collider2D p in players)
        {
            Vector2 dir = (p.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(attackDir, dir);

            if (angle < 45f)
            {
                HeroKnight hero = p.GetComponent<HeroKnight>();
                if (hero != null)
                    hero.TakeDamage(attackDam);
            }
        }
    }

    // ===================== CHIÊU 2 – BẮN =====================
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<BulletEnemy>().SetTarget(Layer.position);
    }

    // ===================== HỖ TRỢ =====================
    protected void Flip()
    {
        isRight = !isRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (attackPoin != null)
        {
            Vector3 p = attackPoin.localPosition;
            p.x *= -1;
            attackPoin.localPosition = p;
        }
    }

    protected void CheckAttackRange()
    {
        isRange = Vector2.Distance(transform.position, Layer.position) <= atackRanger;
    }

    public void EndAttack()
    {
        isAttacking = false;
        currentSkill = SkillState.None;

    }

    // ===================== DAMAGE & DIE =====================
    public void TakeDamage(int damPlayer)
    {
        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);

        intDam++;
        if (intDam == 3)
        {
            animator.SetTrigger("Hurt");
            intDam = 0;
        }

        if (healthnow <= 0)
        {
            animator.SetTrigger("Dead");
            
        }
    }

    protected void Die()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < IntCoin; i++)
            SpawnObject(coinPrefab);

        if (Random.value <= healDropRate)
            SpawnObject(healPrefab);

        if (Poinend != null)
            Poinend.SetActive(true);
    }

    void SpawnObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 force = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atackRanger);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
