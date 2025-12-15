using UnityEngine;

public class Thangmay : MonoBehaviour
{
    [Header("Di chuyển")]
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected Transform start;
    [SerializeField] protected Transform end;

    private Vector2 muctieu;
    private bool moveToEnd = false;
    private bool moveToStart = false;

    void Start()
    {
        transform.position = start.position;
        muctieu = transform.position;
    }

    void Update()
    {
        // Nếu không bật công tắc nào → không di chuyển
        if (!moveToEnd && !moveToStart) return;

        if (moveToEnd)
        {
            muctieu = end.position;
        }
        else if (moveToStart)
        {
            muctieu = start.position;
        }

        transform.position = Vector2.MoveTowards(transform.position,muctieu,speed * Time.deltaTime  );
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

    // ===== GIỮ PLAYER TRÊN SÀN =====
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
