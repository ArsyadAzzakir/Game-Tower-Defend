using UnityEngine;

public class MonkTower : MonoBehaviour
{
    [Header("Attributes")]
    public float range = 3f; 
    public float fireRate = 2f; 
    public int damage = 3;
    private float fireCountdown = 0f;

    [Header("Referensi")]
    public GameObject explosionPrefab; 

    private Transform target;
    private Animator monkAnimator;

    private void Awake()
    {
        monkAnimator = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider2D enemy in enemies)
        {
            EnemyHealth enemyScript = enemy.GetComponent<EnemyHealth>();
            if (enemyScript != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy.transform;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy;
            Debug.Log("CCTV: Monk mengunci target musuh!"); // PELACAK 1
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Debug.Log("CCTV: Monk mulai menyerang!"); // PELACAK 2
            monkAnimator.SetTrigger("Attack"); 
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    public void SpawnExplosionAtTarget()
    {
        Debug.Log("CCTV: Fungsi Ledakan Terpanggil dari Animasi!");

        if (explosionPrefab == null)
        {
            Debug.Log("ERROR: Kolom Explosion Prefab di Inspector masih KOSONG!");
            return;
        }

        if (target == null)
        {
            Debug.Log("INFO: Target sudah mati dipanah duluan, ledakan dibatalkan.");
            return;
        }

        GameObject explosion = Instantiate(explosionPrefab, target.position, Quaternion.identity);
        Debug.Log("CCTV: Ledakan berhasil diciptakan di lokasi target!");
        
        MonkExplosionAoE explosionScript = explosion.GetComponent<MonkExplosionAoE>();
        if (explosionScript != null)
        {
            explosionScript.Setup(damage);
        }
        else
        {
            Debug.Log("ERROR: Prefab ledakan tidak punya skrip MonkExplosionAoE!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}