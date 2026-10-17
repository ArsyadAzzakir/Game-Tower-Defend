using UnityEngine;

public class LancerTurret : MonoBehaviour
{
    public float attackRange = 1f; // Sesuaikan dengan ukuran 1 kotak grid kamu
    public int damage = 300;
    public float attackCooldown = 1.5f; // Lebih lambat dari Monk untuk balancing
    private float attackTimer;

    public LayerMask enemyMask; // Set ke layer tempat musuh berada
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        CheckAndAttack();
    }

    void CheckAndAttack()
    {
        // Mengecek 4 arah persis (Pola +)
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

        foreach (Vector2 dir in directions)
        {
            // Menembakkan sensor sejauh 1 kotak grid
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, attackRange, enemyMask);

            if (hit.collider != null)
            {
                // Musuh terdeteksi di arah ini!
                ExecuteAttack(hit.collider.gameObject, dir);
                attackTimer = attackCooldown;
                break; // Hentikan loop agar Lancer hanya menusuk 1 arah dalam 1 waktu
            }
        }
    }

    void ExecuteAttack(GameObject enemy, Vector2 direction)
    {
        // 1. Atur arah hadap dan picu animasi tusukan
        if (direction == Vector2.up)
        {
            anim.SetTrigger("AttackUp");
        }
        else if (direction == Vector2.down)
        {
            anim.SetTrigger("AttackDown");
        }
        else if (direction == Vector2.right)
        {
            sr.flipX = false;
            anim.SetTrigger("AttackSide");
        }
        else if (direction == Vector2.left)
        {
            sr.flipX = true; // Membalik sprite serangan kanan untuk dipakai di kiri
            anim.SetTrigger("AttackSide");
        }

        // 2. Panggil script EnemyHealth untuk mengurangi HP target
        enemy.GetComponent<EnemyHealth>().TakeDamage(damage); 
    }
}