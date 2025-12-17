using UnityEngine;

public class Thangmay : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    private Vector2 muctieu;
    private bool moveToEnd = false;
    private bool moveToStart = false;

    // ===== GIỮ PLAYER =====
    private Transform player;
    private bool removeParent = false;

    void Start()
    {
        transform.position = start.position;
        muctieu = transform.position;
    }

    void Update()
    {
        // Không di chuyển nếu đang dừng
        if (!moveToEnd && !moveToStart) return;

        if (moveToEnd)
            muctieu = end.position;
        else if (moveToStart)
            muctieu = start.position;

        transform.position = Vector2.MoveTowards(
            transform.position,
            muctieu,
            speed * Time.deltaTime
        );

        // Đến nơi thì dừng
        if (Vector2.Distance(transform.position, muctieu) < 0.01f)
        {
            StopMove();
        }
    }

    // ===== TÁCH PLAYER Ở FRAME AN TOÀN =====
    private void LateUpdate()
    {
        if (removeParent && player != null)
        {
            player.SetParent(null);
            player = null;
            removeParent = false;
        }
    }

    // ===== HÀM GỌI TỪ CÔNG TẮC =====
    public void SwitchToEnd()
    {
        moveToEnd = true;
        moveToStart = false;
    }

    public void SwitchToStart()
    {
        moveToStart = true;
        moveToEnd = false;
    }

    public void StopMove()
    {
        moveToEnd = false;
        moveToStart = false;
    }

    // ===== TRIGGER =====
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            player.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            removeParent = true; 
        }
    }
}
