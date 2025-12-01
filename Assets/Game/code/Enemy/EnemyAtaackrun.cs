using System.Collections;
using UnityEngine;

public class EnemyAtaackrun : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] protected bool isRight = true;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float moveDistance = 4f;
    private bool isStunned = false;
    private float stunTime = 1f;

    [Header("Trạng thái máu")]
    public float health = 100;
    public float healthnow;
    public HPEnemy quai;
    [SerializeField] protected float Damage = 10;

    protected Vector2 startPos;
    protected Rigidbody2D rb;

    [SerializeField] protected Transform Layer;



    [SerializeField] protected Animator animator;
    [Header("Vang")]
    public GameObject coinPrefab;
    public int IntCoin = 2;

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

        Move();

    }

    public void mau()
    {
        healthnow = health;
        quai.capNhatMau(healthnow, health);
    }

    protected virtual void Move()
    {
        if (isStunned) return;

        // Tính hướng
        Vector2 dir = isRight ? Vector2.right : Vector2.left;

        // Di chuyển bằng Rigidbody
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

        // Kiểm tra khoảng cách để flip
        if (isRight && transform.position.x >= startPos.x + moveDistance)
            Flip();
        else if (!isRight && transform.position.x <= startPos.x - moveDistance)
            Flip();
    }


    protected void Flip()
    {
        isRight = !isRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void TakeDamage(int damPlayer)
    {
        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);
        StartCoroutine(Stun());

        if (healthnow <= 0)
        {
            Die();
        }


    }
    private IEnumerator Stun()
    {
        isStunned = true;           // dừng di chuyển
        animator.SetBool("Idea", true);
        yield return new WaitForSeconds(stunTime);
        isStunned = false;          // di chuyển lại
        animator.SetBool("Idea", false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            HeroKnight hero = collision.collider.GetComponent<HeroKnight>();
            if (hero != null)
                hero.TakeDamage(Damage);

        }
    }
    protected void Die()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < IntCoin; i++)
        {
            SpawnObject(coinPrefab);
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
