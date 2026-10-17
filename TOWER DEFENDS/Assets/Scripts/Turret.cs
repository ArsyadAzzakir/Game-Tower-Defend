using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private Animator animator;  

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float fireRate = 1f;

    [Header("Audio Tembakan")]
    public AudioSource sumberSuara;
    public AudioClip suaraSwish;
    private static float waktuBolehBunyiTembak = 0f;

    private Transform currentTarget;
    private float timeUntilFire;

    private void Update()
    {   
        timeUntilFire += Time.deltaTime;
        FindTarget();

        if (currentTarget == null) return; 

        RotateTowardsTarget();

        // === KALKULASI ATTACK SPEED BUFF ===
        // Mengalikan fireRate dasar dengan multiplier dari kartu buff
        float effectiveFireRate = fireRate * BuffManager.main.archerAttackSpeedMultiplier;

        // Cek cooldown menggunakan fireRate yang sudah di-buff
        if (timeUntilFire >= 1f / effectiveFireRate)
        {
            // 1. Update HANYA bertugas menyalakan pelatuk animasi
            if (animator != null) 
            {
                animator.ResetTrigger("Shoot"); // Mencegah penumpukan trigger
                animator.SetTrigger("Shoot");
            }
            timeUntilFire = 0f;
        }
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // === KALKULASI ATTACK RANGE BUFF ===
        // Mengalikan jarak dasar dengan multiplier dari kartu buff
        float effectiveRange = targetingRange * BuffManager.main.archerAttackRangeMultiplier;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            
            // Gunakan effectiveRange yang sudah di-buff untuk mendeteksi musuh
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= effectiveRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null) currentTarget = nearestEnemy.transform;
        else currentTarget = null;
    }

    private void RotateTowardsTarget()
    {
        if (currentTarget.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // 2. Fungsi ini SEKARANG HANYA dipanggil oleh Animation Event dari Animator
    public void Shoot()
    {
        if (currentTarget == null) return;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetTarget(currentTarget);
        
        if (sumberSuara != null && suaraSwish != null)
        {
            if (Time.time >= waktuBolehBunyiTembak)
            {
                sumberSuara.pitch = Random.Range(0.85f, 1.15f);
                sumberSuara.PlayOneShot(suaraSwish, 0.3f); 
                waktuBolehBunyiTembak = Time.time + 0.1f; 
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Agar garis lingkaran di Scene View ikut membesar saat kita dapat buff range
        float currentRange = targetingRange;
        if (Application.isPlaying && BuffManager.main != null)
        {
            currentRange *= BuffManager.main.archerAttackRangeMultiplier;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }
}