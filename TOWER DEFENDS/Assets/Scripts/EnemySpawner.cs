using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 1.2f;

    [Header("Events")]
    public static UnityEvent OnEnemyDestroy = new UnityEvent();

    [Header("Pengaturan Kemunculan Musuh")]
    [Tooltip("Khusus Wave 1 agar Archer sempat menembak")]
    public float wave1MinDelay = 1.2f; 
    public float wave1MaxDelay = 2.5f;
    
    [Tooltip("Untuk Wave 2 dan seterusnya (Mulai berdempetan)")]
    public float minSpawnDelay = 0.2f; 
    public float maxSpawnDelay = 1.5f;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private float currentSpawnDelay; 
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        OnEnemyDestroy.RemoveAllListeners();
        OnEnemyDestroy.AddListener(EnemyDestroyed);
    }
    
    private void Start()
    {
        StartCoroutine(StartWave());
    }
    
    private void Update()
    {   
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;
        
        if (timeSinceLastSpawn >= currentSpawnDelay && enemiesLeftToSpawn > 0 )
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            timeSinceLastSpawn = 0f;
            
            // Acak ulang waktu tunggu menggunakan fungsi pintar
            currentSpawnDelay = GetRandomDelay();
        }
        
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        
        if (WaveNotifier.main != null)
        {
            WaveNotifier.main.TampilkanNotif(currentWave);
        }

        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        
        // Gunakan fungsi pintar untuk musuh pertama
        currentSpawnDelay = GetRandomDelay();
        timeSinceLastSpawn = currentSpawnDelay; 
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        GameObject spawnedEnemy = Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
        EnemyHealth healthComponent = spawnedEnemy.GetComponent<EnemyHealth>();
        
        if (healthComponent != null)
        {
            healthComponent.BoostHealt(currentWave); 
        }

        enemiesAlive++;
    }

    // --- FUNGSI PINTAR UNTUK MENGATUR JEDA ---
    private float GetRandomDelay()
    {
        if (currentWave < 7)
        {
            // Wave 1: Musuh santai dan berjauhan (Beri ampun untuk Archer)
            return Random.Range(wave1MinDelay, wave1MaxDelay);
        }
        else
        {
            // Wave 3 ke atas: Musuh mulai brutal dan berdempetan (Makanan empuk buat Monk)
            return Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }
}