using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float speed = 5f;   
    [Header("Audio Hitmarker")]
    public AudioClip suaraHit;
    private static float waktuBolehBunyiHit = 0f;

    private Transform target;
    private bool sudahKena = false;

    public void SetTarget(Transform _target)
    {
    target = _target;
    Vector2 direction = target.position - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Update ()
{
    if (target == null)
    {
    Destroy(gameObject);
    return;
    }


    transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    
    if (Vector2.Distance(transform.position, target.position) <= 0.5f)
    {
        sudahKena = true; // Gembok langsung dikunci setelah sentuhan pertama
        HitTarget();
    }

}

   private void HitTarget()
    {
        EnemyHealth enemy = target.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            // 1. Kalkulasi Damage Dasar + Buff Damage
            int finalDamage = Mathf.RoundToInt(100 * BuffManager.main.archerDamageMultiplier);

            // 2. Cek apakah serangan ini Critical? (RNG)
            float critChance = BuffManager.main.archerCriticalRateBonus;
            bool isCrit = Random.value <= critChance; 

            // 3. Jika Critical, kalikan damage-nya (misalnya 2x lipat)
            if (isCrit)
            {
                finalDamage *= 2; 
            }

            // 4. Kirim damage ke musuh dan beri tahu kalau ini serangan critical
            // ERROR SEMENTARA: Bagian ini akan error bergaris merah sampai kamu melakukan Langkah 2 di bawah
            enemy.TakeDamage(finalDamage, isCrit);

            if (suaraHit != null)
            {
                if (Time.time >= waktuBolehBunyiHit)
                {
                    AudioSource.PlayClipAtPoint(suaraHit, Camera.main.transform.position, 0.4f);
                    waktuBolehBunyiHit = Time.time + 0.05f; 
                }
            }
        }
        
        Destroy(gameObject); 
    }
}
    