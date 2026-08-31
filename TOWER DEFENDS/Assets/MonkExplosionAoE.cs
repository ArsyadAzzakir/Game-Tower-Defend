using UnityEngine;

public class MonkExplosionAoE : MonoBehaviour
{
    [Header("Pengaturan Area")]
    public float explosionRadius = 0.8f; 
    public float destroyDelay = 0.5f;    

    [Header("Audio Serangan Monk (Berlapis)")]
    public AudioSource sumberSuara;
    public AudioClip suaraMagicBoom;
    public AudioClip suaraSubBass; 
    private static float waktuBolehBunyiMonk = 0f;

    public void Setup(int damageAmount)
    {
        PerformAoEDamage(damageAmount);
    }

    private void PerformAoEDamage(int damageValue)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        // Daftar untuk mengingat musuh mana yang sudah kena damage
        System.Collections.Generic.List<EnemyHealth> musuhYangSudahKena = new System.Collections.Generic.List<EnemyHealth>();

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyScript = enemyCollider.GetComponent<EnemyHealth>();
            
            // Cek jika musuh ada DAN belum ada di dalam daftar
            if (enemyScript != null && !musuhYangSudahKena.Contains(enemyScript))
            {
                enemyScript.TakeDamage(damageValue);
                musuhYangSudahKena.Add(enemyScript); // Masukkan ke daftar
            }
        }

        MainkanAudioLedakan();
        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void MainkanAudioLedakan()
    {
        // 1. Cek: Apakah saat ini terlalu cepat untuk memutar suara lagi?
        // Jika ya, BATALKAN pemutaran suara untuk ledakan yang ini.
        if (Time.time < waktuBolehBunyiMonk) return; 

        // 2. Set waktu baru: Ledakan berikutnya baru boleh bunyi 0.1 detik dari sekarang
        waktuBolehBunyiMonk = Time.time + 0.1f; 

        // 3. Putar audio seperti biasa
        if (sumberSuara != null)
        {
            if (suaraMagicBoom != null)
            {
                sumberSuara.pitch = Random.Range(1.0f, 1.2f);
                sumberSuara.PlayOneShot(suaraMagicBoom, 0.4f); 
            }
            if (suaraSubBass != null)
            {
                sumberSuara.pitch = Random.Range(0.8f, 1.2f);
                sumberSuara.PlayOneShot(suaraSubBass, 0.1f); 
            }
        }
    }
}