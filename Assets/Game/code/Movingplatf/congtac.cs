using UnityEngine;

public class congtac : MonoBehaviour
{
    public Thangmay platform;
    public bool goToEnd = true; // true = đi tới end, false = quay về start

    private bool playerInRange = false;
    private bool isActivated = false;

    void Update()
    {
        // Player đứng trong vùng + bấm J + chưa kích hoạt
        if (playerInRange && !isActivated && Input.GetKeyDown(KeyCode.J))
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
        isActivated = true;

        if (goToEnd)
            platform.SwitchToEnd();
        else
            platform.SwitchToStart();
    }
}
