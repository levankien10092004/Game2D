using System.Collections;
using UnityEngine;

public class EnemyFlyThrower : MonoBehaviour
{
    [Header("Bắn đạn")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackRange = 10f;
    public float attackCooldown = 2f;

    private float lastAttackTime;
    private bool isShooting = false;

    [SerializeField] private Transform player;
    private bool facingRight = false;
    private Rigidbody2D rb;

    [Header("Animator")]
    public Animator animator;

    [Header("Máu")]
    public float health = 100;
    public float healthnow;
    public HPEnemy quai;

    [Header("Rơi đồ")]
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public int IntCoin = 5;
    public float healDropRate = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (quai == null)
            quai = GetComponentInChildren<HPEnemy>();

        healthnow = health;
        if (quai != null)
            quai.capNhatMau(healthnow, health);

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || isShooting) return;

        // Quay mặt
        if (player.position.x > transform.position.x && !facingRight)
            Flip();
        else if (player.position.x < transform.position.x && facingRight)
            Flip();

        float distance = Vector2.Distance(transform.position, player.position);

        // ==== LOGIC GIỐNG BOSS2 ====
        if (distance <= attackRange)
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                animator.SetBool("Attack", true);
                
                lastAttackTime = Time.time;
            }
        }
    }

    // ===== BẮN ĐẠN =====
    void Shoot()
    {
        isShooting = true;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); 
        // bullet.transform.rotation = Quaternion.Euler(0, 0, -90);
        bullet.GetComponent<BulletEnemy>().SetTarget(player.position);
        StartCoroutine(StopShoot());
    }

    IEnumerator StopShoot()
    {
        yield return new WaitForSeconds(1.2f); // thời gian animation attack
        isShooting = false;
        animator.SetBool("Attack", false);
    }

    // ===== NHẬN DAMAGE =====
    public void TakeDamage(int damPlayer)
    {
        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);

        if (healthnow <= 0)
        {
            animator.SetTrigger("Dead");
            Die();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected void Die()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < IntCoin; i++)
            SpawnObject(coinPrefab);

        if (Random.value <= healDropRate)
            SpawnObject(healPrefab);
    }

    void SpawnObject(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        Rigidbody2D rb2 = obj.GetComponent<Rigidbody2D>();

        if (rb2 != null)
        {
            Vector2 force = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb2.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
