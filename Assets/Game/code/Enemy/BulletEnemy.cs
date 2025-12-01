using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float speed = 1f;
    public float arcHeight = 2f;
    public float damage = 10f;
    public float lifeTime = 5f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float t = 0f;

    public LayerMask groundLayer; // Thêm layer Ground

    public void SetTarget(Vector3 target)
    {
        startPos = transform.position;

        // --- TÌM MẶT ĐẤT DƯỚI HÒA ĐỘ X CỦA TARGET ---
        RaycastHit2D hit = Physics2D.Raycast( new Vector2(target.x, target.y + 5f),  Vector2.down, 20f, groundLayer );

        if (hit.collider != null)
        {
            // Nếu raycast trúng đất → đặt target xuống đất
            targetPos = new Vector3(target.x, hit.point.y, transform.position.z);
        }
        else
        {
            // Nếu không tìm thấy đất → mặc định rơi xuống dưới chút
            targetPos = new Vector3(target.x, target.y - 5f, transform.position.z);
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        t += Time.deltaTime * speed;

        Vector3 midPoint = (startPos + targetPos) * 0.5f;
        midPoint.y += arcHeight;

        Vector3 a = Vector3.Lerp(startPos, midPoint, t);
        Vector3 b = Vector3.Lerp(midPoint, targetPos, t);

        transform.position = Vector3.Lerp(a, b, t);

        // Nếu đã đến đích thì tự xoá để tránh kẹt
        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

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
