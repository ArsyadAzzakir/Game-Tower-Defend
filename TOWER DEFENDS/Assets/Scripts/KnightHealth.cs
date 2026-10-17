using UnityEngine;

public class KnightHealth : MonoBehaviour
{
    [Header("Status Nyawa Knight")]
    public int maxHealth = 1000; // Contoh total nyawa, silakan ubah sesuai keinginan
    private int currentHealth;

    [Header("Efek Kematian")]
    public GameObject smokeEffectPrefab; // Isi dengan prefab efek asap yang sama dengan punya musuh

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 1. Munculkan efek asap persis seperti musuh
        if (smokeEffectPrefab != null)
        {
            Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
        }

        // 2. Hancurkan Knight
        Destroy(gameObject);
    }
}