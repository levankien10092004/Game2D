using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private float lifeTime = 5f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float t;

    public void SetTarget(Vector3 target)
    {
        startPos = transform.position;
        t = 0f;

        //  XOAY NGAY LÚC BẮN 
        Vector3 shootDir = (target - startPos).normalized;
        float startAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg-90f;
        transform.rotation = Quaternion.Euler(0, 0, startAngle);

        //  TÌM MẶT ĐẤT DƯỚI PLAYER 
        RaycastHit2D hit = Physics2D.Raycast( new Vector2(target.x, target.y + 5f),Vector2.down, 20f, groundLayer );

        if (hit.collider != null)
            targetPos = new Vector3(target.x, hit.point.y, transform.position.z);
        else
            targetPos = new Vector3(target.x, target.y - 5f, transform.position.z);

        Destroy(gameObject, lifeTime);
    }

    //  DI CHUYỂN + XOAY 
    private void Update()
    {
        t += Time.deltaTime * speed;
        t = Mathf.Clamp01(t);

       
        Vector3 midPoint = (startPos + targetPos) * 0.5f;
        midPoint.y += arcHeight;

        Vector3 a = Vector3.Lerp(startPos, midPoint, t);
        Vector3 b = Vector3.Lerp(midPoint, targetPos, t);
        Vector3 newPos = Vector3.Lerp(a, b, t);

        //  XOAY THEO HƯỚNG BAY 
        Vector3 dir = newPos - transform.position;
        if (dir.sqrMagnitude > 0.0001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg-90f;
            transform.rotation = Quaternion.Lerp(   transform.rotation, Quaternion.Euler(0, 0, angle),Time.deltaTime * 15f);
        }

        transform.position = newPos;

        if (t >= 3f)
            Destroy(gameObject);
    }

    //  VA CHẠM 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HeroKnight hero = collision.GetComponent<HeroKnight>();
            if (hero != null)
                hero.TakeDamage(damage);

            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
