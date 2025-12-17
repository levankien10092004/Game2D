using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    private bool collected = false;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager =GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collected) return;

        PlayerCoins pc = collision.collider.GetComponent<PlayerCoins>();
        if (pc != null)
        {
            audioManager.PlaySFX(audioManager.Coin);
            collected = true;
            pc.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }

}

