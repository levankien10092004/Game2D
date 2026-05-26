using UnityEngine;

public class SpikedBallTrap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform chainPoint;        // Điểm treo (pivot)
    [SerializeField] private GameObject spikedBallPrefab; // Prefab quả cầu gai

    [Header("Swing Settings")]
    [SerializeField] private float radius = 3f;           // Độ dài dây / bán kính chuyển động
    [SerializeField] private float angleRange = 80f;      // Góc vung qua lại
    [SerializeField] private float startingAngle = 0f;    // Góc ban đầu (âm/dương đều OK)
    [SerializeField] private float rotationSpeed = 140f;  // Tốc độ xoay/swing
    [SerializeField] private bool loop = true;            // true = đu qua lại, false = xoay 1 chiều
    [SerializeField] private bool clockwise = false;      // Định chiều khi xoay một chiều

    private float currentAngle;
    private float direction = 1f;       // 1 hoặc -1
    private Transform ball1;
    private Transform ball2;

    void Start()
    {
        // Spawn quả cầu và đặt nó làm con của object chứa script
        ball1 = Instantiate(spikedBallPrefab, transform).transform;
        ball2 = Instantiate(spikedBallPrefab, transform).transform;

        currentAngle = startingAngle;
    }

    void Update()
    {
        if (loop)
        {
            // Swing qua lại
            currentAngle += rotationSpeed * Time.deltaTime * direction;

            if (Mathf.Abs(currentAngle) >= angleRange)
                direction *= -1f;
        }
        else
        {
            // Quay tròn một hướng
            currentAngle += rotationSpeed * Time.deltaTime * (clockwise ? -1f : 1f);
        }

        float rad1 = currentAngle * Mathf.Deg2Rad;
        float rad2 = (currentAngle+180) * Mathf.Deg2Rad;


        Vector2 offset1 = new Vector2( Mathf.Sin(rad1),   -Mathf.Cos(rad1) );
        Vector2 offset2 = new Vector2(Mathf.Sin(rad2), -Mathf.Cos(rad2));

        ball1.position = chainPoint.position + (Vector3)(offset1 * radius);
        ball2.position = chainPoint.position + (Vector3)(offset2* radius);
    }
}
