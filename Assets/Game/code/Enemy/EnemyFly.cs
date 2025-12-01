using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class EnemyFly : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] protected bool isRight = true;
    [SerializeField] protected float speed = 2f;
    [SerializeField] protected float moveDistance = 4f;

    [Header("Trạng thái máu")]
    public float health = 10;
    public float healthnow;
    public HPEnemy quai;
    protected float chaseSpeed = 3f;
    private bool isAttacking = false;

    [SerializeField] protected float attackDistan = 1.6f;

    protected Vector2 startPos;
    protected Rigidbody2D rb;

    [SerializeField] protected Transform Layer;
    [SerializeField] protected bool isRange = false;
    protected float atackRanger = 6f;

    float offsetY = 1.5f;

    [SerializeField] protected Animator animator;

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
            // Đổi hướng về phía người chơi
            if ((Layer.position.x > transform.position.x && !isRight) ||
                (Layer.position.x < transform.position.x && isRight))
            {
                Flip();
            }

            Vector3 targetPos = new Vector3(Layer.position.x, Layer.position.y + offsetY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, chaseSpeed * Time.deltaTime);
        }
        else
        {
            // --- QUÁI TUẦN TRA ---
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
    }
    protected void AtackRanger()
    {
        if (Vector2.Distance(transform.position, Layer.position) <= atackRanger)
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
        healthnow -= damPlayer;
        if (quai != null)
            quai.capNhatMau(healthnow, health);
        if (healthnow <= 0)
        {
            Die();
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Layer.GetComponent<HeroKnight>().TakeDamage(20);
            Die();
        }
    }
    protected void Die()
    {
        gameObject.SetActive(false);
    }
}
