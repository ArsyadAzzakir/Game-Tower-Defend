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
        
        // Ambil durasi slow dari BuffManager (akan bernilai 0 jika belum punya kartunya)
        float slowDuration = BuffManager.main.monkSlowDuration;
        
        System.Collections.Generic.List<EnemyHealth> musuhYangSudahKena = new System.Collections.Generic.List<EnemyHealth>();

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyScript = enemyCollider.GetComponent<EnemyHealth>();
            
            if (enemyScript != null && !musuhYangSudahKena.Contains(enemyScript))
            {
                enemyScript.TakeDamage(damageValue);
                
                // === EKSEKUSI EFEK SLOW ===
                // Jika pemain sudah mengambil kartu Frost Aura (durasi > 0)
                if (slowDuration > 0)
                {
                    // Kirim perintah melambat ke musuh (kita akan buat fungsinya di script musuh)
                    enemyScript.ApplySlow(slowDuration);
                }

                musuhYangSudahKena.Add(enemyScript);
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
        if (Time.time < waktuBolehBunyiMonk) return; 

        waktuBolehBunyiMonk = Time.time + 0.1f; 

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