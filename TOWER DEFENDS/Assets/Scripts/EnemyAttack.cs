using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Status Serangan")]
    public int attackDamage = 100; // Damage tetap 100
    public float attackCooldown = 1.5f; // Kecepatan normal serangan
    public float attackRange = 0.8f;
    
    [Header("Target")]
    public LayerMask defenderLayer; // Set ke layer "Defender" di Inspector!

    private float cooldownTimer;
    private Animator anim;
    private EnemyMovement movementScript; 

    void Start()
    {
        anim = GetComponent<Animator>();
        movementScript = GetComponent<EnemyMovement>(); 
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        // Sensor mencari Knight di layer Defender
        Collider2D hitDefender = Physics2D.OverlapCircle(transform.position, attackRange, defenderLayer);

        if (hitDefender != null) // Jika ada Knight yang menutupi jalan
        {
            // 1. Matikan script pergerakan agar musuh berhenti
            if (movementScript != null) movementScript.enabled = false;

            // 2. Lakukan serangan jika cooldown sudah siap
            if (cooldownTimer <= 0)
            {
                anim.SetTrigger("Attack"); // Memicu animasi Pawn_Interact Knife.png
                
                KnightHealth knight = hitDefender.GetComponent<KnightHealth>();
                if (knight != null)
                {
                    knight.TakeDamage(attackDamage); // Beri damage 100
                }
                
                cooldownTimer = attackCooldown; // Reset cooldown
            }
        }
        else // Jika Knight sudah mati jadi asap
        {
            // Musuh kembali berjalan
            if (movementScript != null) movementScript.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}