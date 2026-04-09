using UnityEngine;
using System.Collections;

public class EnemyLaserAuto : MonoBehaviour
{
    [Header("Laser Components")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float laserLength = 15f;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private LayerMask hitLayers;

    [Header("Laser Timing")]
    [SerializeField] private float fireTime = 2f;      // thời gian bắn
    [SerializeField] private float cooldownTime = 1f;  // thời gian nghỉ giữa mỗi phát

    private LineRenderer lr;
    private bool hasHitPlayer = false;



    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    
        lr.sortingLayerName = "Effects";
        lr.sortingOrder = 1000;

        lr.startColor = Color.red;
        lr.endColor = Color.red;
        if (lr.material != null)
            lr.material.color = Color.red;

        // Bắt đầu auto fire
        StartCoroutine(LaserRoutine());
    }
    
    
    IEnumerator LaserRoutine()
    {
        while (true)
        {
           
            // ★ 1. BẮN LASER
            lr.enabled = true;

            float timer = 0f;
            while (timer < fireTime)
            {
                timer += Time.deltaTime;

                // Bắn theo hướng firePoint đang nhìn
                Vector2 dir = firePoint.right;   // firePoint.right = bắn ngang
                                                 // firePoint.up nếu muốn bắn dọc

                // Raycast kiểm tra vật va
                RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, laserLength, hitLayers);

                Vector3 endPos;

                if (hit.collider != null)
                {
                    endPos = hit.point;

                    if (!hasHitPlayer)
                    {
                        HeroKnight hero = hit.collider.GetComponent<HeroKnight>();
                        if (hero != null)
                            hero.TakeDamage(10f); // damage 1 lần

                        hasHitPlayer = true;
                    }
                }
                else
                {
                    endPos = firePoint.position + (Vector3)dir * laserLength;
                    hasHitPlayer = false;
                }

                // Vẽ laser
                lr.SetPosition(0, firePoint.position);
                lr.SetPosition(1, endPos);

                yield return null;
            }

            // ★ 2. TẮT LASER
            lr.enabled = false;
            hasHitPlayer = false;

            // ★ 3. NGHỈ
            yield return new WaitForSeconds(cooldownTime);
        }
    }
}
