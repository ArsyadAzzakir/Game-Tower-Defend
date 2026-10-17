using UnityEngine;
using System.Collections; // Wajib ditambahkan untuk menggunakan Coroutine

public class KnightAttack : MonoBehaviour
{
    [Header("Status Serangan")]
    public int damagePerHit = 125; // 250 dibagi 2 tebasan
    public float attackCooldown = 1.5f;
    public float attackRange = 1f;
    
    [Header("Pengaturan Combo")]
    public float delayBetweenHits = 0.3f; // Jeda waktu (detik) antara tebasan ke-1 dan ke-2
    public LayerMask enemyLayer;

    private float cooldownTimer = 0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            CheckAndAttack();
        }
    }

    void CheckAndAttack()
    {
        // Cek apakah ada musuh di dekatnya
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            Transform targetEnemy = hitEnemies[0].transform;

            if (targetEnemy.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            else if (targetEnemy.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            // 1. Putar animasi combo (gabungan)
            anim.SetTrigger("Attack");

            // 2. Jalankan fungsi penundaan untuk membagi damage
            StartCoroutine(PerformDoubleHit());

            // 3. Mulai ulang waktu jeda serangan
            cooldownTimer = attackCooldown;
        }
    }

    // Fungsi Coroutine untuk mengeksekusi dua tebasan beruntun
    IEnumerator PerformDoubleHit()
    {
        // TEBASAN PERTAMA (Langsung saat animasi dimulai)
        DealDamageToEnemies();

        // Tunggu sebentar sampai pedang mengayun untuk tebasan kedua
        yield return new WaitForSeconds(delayBetweenHits);

        // TEBASAN KEDUA
        DealDamageToEnemies();
    }

    // Fungsi untuk memberikan damage ke semua musuh di area
    void DealDamageToEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            // PERHATIAN: Sesuaikan 'EnemyHealth' dengan nama script darah musuhmu
            enemy.GetComponent<EnemyHealth>().TakeDamage(damagePerHit);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}