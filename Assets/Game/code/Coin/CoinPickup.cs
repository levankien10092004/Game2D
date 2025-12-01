using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    private bool collected = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collected) return;

        PlayerCoins pc = collision.collider.GetComponent<PlayerCoins>();
        if (pc != null)
        {
            collected = true;
            pc.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }

}

