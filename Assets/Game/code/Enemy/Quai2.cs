using System.Collections;
using UnityEngine;

public class Quai2 : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] protected bool isRight = true;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float moveDistance = 4f;

    [Header("Trạng thái máu")]
    public int health = 100;
    public int healthnow;
    public HPEnemy quai;

    protected Vector2 startPos;
    protected Rigidbody2D rb;

    [SerializeField] protected Transform Layer;
    protected bool isRange = false;
    protected float atackRanger = 4f;
    [SerializeField] protected int intDam = 0;

    [SerializeField] protected float attackDistan = 1.6f;
    protected float chaseSpeed = 3f;

    [SerializeField] protected Animator animator;

    [Header("Tan cong")]
    [SerializeField] protected Transform attackPoin;
    public float attackDam = 5f;
    public float attackRadius = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;
    private bool isAttacking = false;

    public LayerMask attackLayer;

    [Header("Qua")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 5;
    public float healDropRate = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;
        if (quai == null)
            quai = GetComponentInChildren<HPEnemy>();

        if (quai != null)
            mau();
    }

    void Update()
    {
        AtackRanger();
        Move();
    }

    public void mau()
    {
        healthnow = health;
        quai.capNhatMau(healthnow, health);
    }

    protected virtual void Move()
    {
        if (isRange)
        {
            if (Layer.position.x > transform.position.x && isRight == false)
                Flip();
            else if (Layer.position.x < transform.position.x && isRight == true)
                Flip();

            if (Vector2.Distance(transform.position, Layer.position) > attackDistan)
            {
                animator.SetBool("Attack1", false);
                animator.SetBool("idea", false);
                transform.position = Vector2.MoveTowards(transform.position, Layer.position, chaseSpeed * Time.deltaTime);
                isAttacking = false;
            }
            else
            {
                if (!isAttacking && Time.time - lastAttackTime >= attackCooldown)
                {
                    animator.SetBool("Attack1", true);
                    isAttacking = true;
                    lastAttackTime = Time.time;
                    animator.SetBool("idea", true);
                }
            }
        }
        else
        {
            Vector2 dir = isRight ? Vector2.right : Vector2.left;
            transform.Translate(dir * speed * Time.deltaTime);

            if (isRight && transform.position.x >= startPos.x + moveDistance)
                Flip();
            else if (!isRight && transform.position.x <= startPos.x - moveDistance)
                Flip();
        }
    }

    protected void Flip()
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        if (attackPoin != null)
        {
            Vector3 pointScale = attackPoin.localPosition;
            pointScale.x *= -1;
            attackPoin.localPosition = pointScale;
        }
    }

    protected void AtackRanger()
    {
        isRange = Vector2.Distance(transform.position, Layer.position) <= atackRanger;
    }

    // 🔥🔥🔥 HÀM TẤN CÔNG GIỐNG PLAYER — HOẠT ĐỘNG 100%
    protected void attack()
    {
        // hướng quái đang nhìn
        Vector2 attackDir = isRight ? Vector2.right : Vector2.left;

        // quét player trong vùng tròn
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayer);

        foreach (Collider2D p in players)
        {
            Vector2 dir = (p.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(attackDir, dir);

            // player nằm trong góc phía trước (45 độ)
            if (angle < 45f)
            {
                HeroKnight hero = p.GetComponent<HeroKnight>();
                if (hero != null)
                {
                    hero.TakeDamage(attackDam);
                   
                }
            }
        }
    }

    public void TakeDamage(int damPlayer)
    {
        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);
        intDam += 1;

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
        for(int i=0; i < IntCoin; i++)
        {
            SpawnObject(coinPrefab);
        }
        
        float dropRate = Random.value; // 0.0 → 1.0
        if (dropRate <= healDropRate)
        {
            SpawnObject(healPrefab);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoin == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void EndAttack()
    {
        animator.SetBool("Attack1", false);
        isAttacking = false;
    }
    void SpawnObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }
}
