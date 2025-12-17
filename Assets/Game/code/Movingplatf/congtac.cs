using UnityEngine;

public class congtac : MonoBehaviour
{
    [Header("Tham chiếu")]
    public Thangmay platform;

    [Header("Thiết lập")]
    public bool goToEnd = true; // true = đi tới end, false = quay về start
    public bool oneTimeUse = false; // bật nếu công tắc chỉ dùng 1 lần

    private bool playerInRange = false;
    private bool isActivated = false;

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.J))
        {
            ActivateSwitch();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void ActivateSwitch()
    {
        if (oneTimeUse && isActivated) return;

        isActivated = true;

        if (platform == null)
        {
            return;
        }

        if (goToEnd)
            platform.SwitchToEnd();
        else
            platform.SwitchToStart();
    }
}
