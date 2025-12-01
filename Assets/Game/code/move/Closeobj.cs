using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closeobj : MonoBehaviour
{
    public Transform ruong;       // Điểm spawn

    private bool isOpened = false;
    private bool playerInRange = false;
    private PlayerCoins playerCoins;

    void Update()
    {
        if (playerInRange && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerCoins = other.GetComponent<PlayerCoins>();
        if (playerCoins != null)
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCoins>() != null)
        {
            playerInRange = false;

        }
    }

    void OpenChest()
    {
        isOpened = true;
        ruong.gameObject.SetActive(false);


    }
}
