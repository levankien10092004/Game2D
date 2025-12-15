using UnityEngine;
using System.Collections;

public class EnemyLaserAuto : MonoBehaviour
{
    [Header("Laser Components")]
    public Transform firePoint;
    public float laserLength = 15f;
    public float damagePerSecond = 10f;
    public LayerMask hitLayers;

    [Header("Laser Timing")]
    public float fireTime = 2f;      // thời gian bắn
    public float cooldownTime = 1f;  // thời gian nghỉ giữa mỗi phát

    private LineRenderer lr;

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

                    // Nếu player trúng laser → gây sát thương mỗi frame
                    if (hit.collider.CompareTag("Player"))
                    {
                        HeroKnight hero = hit.collider.GetComponent<HeroKnight>();
                        if (hero != null)
                            hero.TakeDamage((int)(damagePerSecond * Time.deltaTime));
                    }
                }
                else
                {
                    endPos = firePoint.position + (Vector3)dir * laserLength;
                }

                // Vẽ laser
                lr.SetPosition(0, firePoint.position);
                lr.SetPosition(1, endPos);

                yield return null;
            }

            // ★ 2. TẮT LASER
            lr.enabled = false;

            // ★ 3. NGHỈ
            yield return new WaitForSeconds(cooldownTime);
        }
    }
}
