using UnityEngine;

public class EnemyThrower : MonoBehaviour
{
    [Header("Cấu hình ném")]
    public GameObject bulletPrefab;     // Prefab đạn
    public Transform firePoint;         // Vị trí tạo đạn
    public float attackRange = 10f;      // Khoảng cách tấn công
    public float attackCooldown = 2f;   // Thời gian hồi chiêu

    private float timer;
    [SerializeField] private Transform player;
    private bool facingRight = false;
    protected Rigidbody2D rb;

    public Animator animator;

    [Header("Trạng thái máu")]
    public float health = 50;
    public float healthnow;
    public HPEnemy quai;
    [Header("Qua")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 5;
    public float healDropRate = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = attackCooldown;
        if (quai == null)
            quai = GetComponentInChildren<HPEnemy>();

        if (quai != null)
            mau();
    }

    private void Update()
    {
        if (player == null) return;

        timer -= Time.deltaTime;

        // --- Thêm: quay mặt về phía người chơi ---
        if (player.position.x > transform.position.x && !facingRight)
            Flip();
        else if (player.position.x < transform.position.x && facingRight)
            Flip();

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Chạy animation tấn công
            if (animator != null)
                animator.SetBool("Attack",true);

            // Bắn đạn nếu cooldown xong
            if (timer <= 0f)
            {
                ThrowBullet();
                timer = attackCooldown;
            }
        }
        else
        {
            animator.SetBool("Attack", false);
        }

    }
    public void TakeDamage(int damPlayer)
    {
        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);
        if (healthnow <= 0)
        {
            Die();
        }


    }
    public void mau()
    {
        healthnow = health;
        quai.capNhatMau(healthnow, health);
    }

    void ThrowBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<BulletEnemy>().SetTarget(player.position);   // người chơi là mục tiêu
    }

    // --- Giữ nguyên logic Flip ---
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Optional: hiển thị hướng firePoint
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(firePoint.position, firePoint.position + Vector3.right * (facingRight ? 1 : -1));
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

