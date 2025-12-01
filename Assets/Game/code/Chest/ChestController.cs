using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Animator animator;
    public GameObject coinPrefab;      // Prefab đồng xu
    public GameObject healPrefab;      // ⭐ Prefab máu
    public float healDropRate = 0.2f;
    public Transform spawnPoint;       // Điểm spawn
    public int minCoins = 5;
    public int maxCoins = 6;

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
            Debug.Log("Nhấn E để mở rương");
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
        animator.SetTrigger("Open");

        int randomCoin = Random.Range(minCoins, maxCoins + 1);

        // Tạo coin bay ra
        for (int i = 0; i < randomCoin; i++)
        {
            SpawnObject(coinPrefab);
        }

        // ⭐ Tỉ lệ 20% rơi vật phẩm hồi máu ⭐
        float dropRate = Random.value; // 0.0 → 1.0
        if (dropRate <= healDropRate)
        {
            SpawnObject(healPrefab);
        }
    }

    // Hàm spawn kèm lực bay
    void SpawnObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 randomForce = new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f));
            rb.AddForce(randomForce, ForceMode2D.Impulse);
        }
    }
}
