using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyAttack : MonoBehaviour
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
    private bool isAttacking = false; // Trạng thái đang bổ nhào tấn công
    private bool isReturning = false; // Trạng thái đang quay về vị trí bắt đầu
    private bool isDead = false;

    [SerializeField] protected float attackDistan = 5f;

    protected Vector2 startPos;
    protected Rigidbody2D rb;
    protected Vector3 attackStartPos; // Lưu vị trí khi bắt đầu tấn công

    [SerializeField] protected Transform Layer;
    [SerializeField] protected bool isRange = false;
    [SerializeField] protected float atackRanger = 10f;

    float offsetY = 1.5f;

    [SerializeField] protected Animator animator;

    // --- Điều khiển Tấn công ---
    [Header("Tấn công & Sát thương")]
    [SerializeField] protected float attackCooldown = 2f;
    protected float lastAttackTime;
    [SerializeField] public float collisionDamage = 20f; // Sát thương gây ra khi va chạm
    [SerializeField] public float pounceForce = 8f; // Tốc độ bổ nhào (Lao vào)
    [SerializeField] LayerMask Map;
    private bool isGrounded;

    [Header("Qua")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 5;  
    public float healDropRate = 0.2f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        if (quai == null) quai = GetComponentInChildren<HPEnemy>();  
        if (quai != null) mau();

        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Animator not found on " + gameObject.name);
    }

    void Update()
    {
        if (isDead) return;

        AtackRanger();

        // Chỉ di chuyển/tấn công khi không đang thực hiện animation tấn công
        if (!isAttacking)
        {
            Move();
        }

        // Kiểm tra và thực hiện tấn công nếu trong tầm
        if (isRange && Time.time > lastAttackTime + attackCooldown)
        {
            // Quái vật phải nằm trong phạm vi attackDistan và KHÔNG đang quay về
            if (!isReturning && Vector2.Distance(transform.position, Layer.position) <= attackDistan)
            {
                StartCoroutine(Attack()); 
            }
        }
        CheckGround();   
    }
     
    public void mau()
    {
        healthnow = health;
        // Giả định HPEnemy có hàm capNhatMau
        if (quai != null) quai.capNhatMau(healthnow, health);
    }

    protected virtual void Move()
    {
        // 1. Logic QUAY VỀ SAU TẤN CÔNG (Ưu tiên cao nhất)
        if (isReturning)
        {
            if (animator != null) animator.SetBool("Attack", false);

            // Di chuyển về vị trí đã lưu (dùng transform vì nó đang bay lơ lửng)
            transform.position = Vector3.MoveTowards(transform.position, attackStartPos, chaseSpeed * Time.deltaTime);

            // Kiểm tra nếu đã về đích
            if (Vector3.Distance(transform.position, attackStartPos) < 0.1f)
            {
                isReturning = false; // Kết thúc quay về
            }
            return;
        }

        // 2. Logic ĐUỔI THEO (Bay theo phương ngang)
        if (isRange)
        {
            if (animator != null)
            {
                // Đặt animation bay bình thường khi đuổi theo
                animator.SetBool("Attack", false);
            }

            // Đổi hướng về phía người chơi
            if ((Layer.position.x > transform.position.x && !isRight) ||
                (Layer.position.x < transform.position.x && isRight))
            {
                Flip();
            }

            // Bay theo phương ngang: Chỉ theo X của Player, giữ nguyên Y hiện tại
            Vector3 targetPos = new Vector3(Layer.position.x, transform.position.y, transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);

        }
        // 3. Logic TUẦN TRA
        else
        {
            // QUÁI TUẦN TRA
            if (animator != null)
            {
                animator.SetBool("Attack", false);
            }

            Vector2 dir = isRight ? Vector2.right : Vector2.left;
            transform.Translate(dir * speed * Time.deltaTime);

            if (isRight && transform.position.x >= startPos.x + moveDistance)
                Flip();
            else if (!isRight && transform.position.x <= startPos.x - moveDistance)
                Flip();
        }
    }

    // Hàm Tấn công (Coroutine)
    protected IEnumerator Attack()
    {
        attackStartPos = transform.position;

        isAttacking = true;
        lastAttackTime = Time.time;

        if (animator != null)
        {
            // Đảm bảo animation luôn chạy khi tấn công
            animator.SetBool("Attack", true);
        }

        // --- LAO VÀO NHÂN VẬT ---
        // Tính toán hướng lao tới
        Vector2 direction = (Layer.position - transform.position).normalized;
        // Gán vận tốc để bổ nhào
        rb.velocity = direction * pounceForce;

        // Thời gian animation bổ nhào
        yield return new WaitForSeconds(0.8f);

        // --- KẾT THÚC LAO VÀO ---
        rb.velocity = Vector2.zero; // Dừng chuyển động ngay lập tức

        // Nếu Coroutine chạy hết mà không có va chạm:
        isAttacking = false;

        if (animator != null)
        {
            animator.SetBool("Attack", false);
        }

        // Bắt đầu quá trình quay về vị trí tấn công
        isReturning = true;
    }

    // Hàm lật hướng
    protected void Flip()
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // Hàm kiểm tra tầm hoạt động
    protected void AtackRanger()
    {
        if (Layer != null && Vector2.Distance(transform.position, Layer.position) <= atackRanger)
        {
            isRange = true;
        }
        else
        {
            isRange = false;
        }
    }

    public void TakeDamage(int damPlayer)
    {
        if (isDead) return;

        healthnow -= damPlayer;

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        if (quai != null)
            quai.capNhatMau(healthnow, health);

        if (healthnow <= 0)
        {
            animator.SetTrigger("Dead");
            Die();
        }
    }

    // Logic va chạm
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // CHỈ XỬ LÝ NẾU ĐANG TRONG TRẠNG THÁI BỔ NHÀO (Attack)
            if (isAttacking)
            {
                // 1. Ngừng Coroutine và dừng bổ nhào
                StopCoroutine("Attack");
                rb.velocity = Vector2.zero; // Dừng velocity khi va chạm

                // 2. Gây sát thương (Giả định Layer có component HeroKnight)
                HeroKnight hero = Layer.GetComponent<HeroKnight>();
                if (hero != null)
                {
                    hero.TakeDamage((int)collisionDamage);
                }

                // 3. Đặt cờ trạng thái
                isAttacking = false;
                isReturning = true; // Kích hoạt trạng thái quay về

                // 4. Reset animation
                if (animator != null)
                {
                    animator.SetBool("Attack", false);
                }
            }
        }
    }
    void CheckGround()
    {
        // Raycast xuống dưới 0.5f
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.5f, Map);

        // Nếu chạm đất → reset trạng thái tấn công
        if (isGrounded)
        {
            isAttacking = false;
            isReturning = true;
            rb.velocity = Vector2.zero;

            if (animator != null)
                animator.SetBool("Attack", false);
        }
    }

    protected void Die()
    {
    
            gameObject.SetActive(false);
        SpawnObject(coinPrefab);
        float dropRate = Random.value; // 0.0 → 1.0
        if (dropRate <= healDropRate)
        {
            SpawnObject(healPrefab);
        }

    }

    // Hiển thị phạm vi trong Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Vẽ phạm vi đuổi theo
        Gizmos.DrawWireSphere(transform.position, atackRanger);

        Gizmos.color = Color.red;
        // Vẽ phạm vi tấn công
        Gizmos.DrawWireSphere(transform.position, attackDistan);
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