using System.Collections;
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

        if (timeUntilFire >= 1f / fireRate)
        {
            Shoot();
            timeUntilFire = 0f;
        }
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= targetingRange)
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

    private void Shoot()
    {
        if(animator != null) 
        {
            animator.SetTrigger("Shoot");
        }
        
        StartCoroutine(DelayShoot());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }

    private IEnumerator DelayShoot()
    {
        // Menunggu 0.6 detik sampai tangan archer melepaskan tali busur
        yield return new WaitForSeconds(0.6f); 
        
        if (currentTarget != null)
        {
            // 1. Munculkan panah
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
            arrow.GetComponent<Arrow>().SetTarget(currentTarget);
            
            // 2. Putar suara swish TEPAT saat panah muncul
            if (sumberSuara != null && suaraSwish != null)
            {
                // Cek apakah suara boleh diputar
                if (Time.time >= waktuBolehBunyiTembak)
                {
                    sumberSuara.pitch = Random.Range(0.85f, 1.15f);
                    sumberSuara.PlayOneShot(suaraSwish, 0.3f); 
                    
                    // Set jeda 0.1 detik untuk semua pemanah lain
                    waktuBolehBunyiTembak = Time.time + 0.1f; 
                }
            }
        }
    }
}