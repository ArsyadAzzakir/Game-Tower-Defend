using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private Animator anim; 

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    public int attackDamage = 100; 
    public float attackCooldown = 1.5f; 

    private Transform target;
    private int pathIndex = 0;

    [Header("Sensor Knight (Defender)")]
    public LayerMask defenderLayer; 
    public float stopDistance = 0.6f; 
    
    private float currentSpeed;
    private float lastAttackTime;
    private float originalSpeed;

    private void Start() 
    {
        anim = GetComponent<Animator>();
        target = LevelManager.main.path[pathIndex];
        currentSpeed = moveSpeed;
        originalSpeed = moveSpeed;
    }

    private void Update()
    {
        if (target == null) return;

        if (currentSpeed > 0f)
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f) 
            {
                pathIndex++;
                
                if (pathIndex == LevelManager.main.path.Length)
                {
                    GameManager.main.BaseTakeDamage(1);
                    EnemySpawner.OnEnemyDestroy.Invoke();
                    Destroy(gameObject);
                    return;
                } 
                else
                {
                    target = LevelManager.main.path[pathIndex];
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        Collider2D hitDefender = Physics2D.OverlapCircle(transform.position, stopDistance, defenderLayer);

        if (hitDefender != null)
        {
            currentSpeed = 0f; 

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                if (anim != null) anim.SetTrigger("Attack"); 
                
                KnightHealth knight = hitDefender.GetComponent<KnightHealth>();
                if (knight != null)
                {
                    knight.TakeDamage(attackDamage);
                }
                lastAttackTime = Time.time;
            }
        }
        else
        {
            currentSpeed = originalSpeed; 
        }

        rb.linearVelocity = direction * currentSpeed;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    // === FUNGSI BARU UNTUK EFEK SLOW DARI MONK ===
    public void ApplySlowEffect(float slowMultiplier)
    {
        originalSpeed = moveSpeed * slowMultiplier;
        // Jika musuh sedang jalan (tidak dihadang Knight), langsung lambatkan!
        if (currentSpeed > 0f) currentSpeed = originalSpeed; 
    }

    public void RemoveSlowEffect()
    {
        originalSpeed = moveSpeed;
        // Kembalikan ke kecepatan jalan normal
        if (currentSpeed > 0f) currentSpeed = originalSpeed; 
    }
}