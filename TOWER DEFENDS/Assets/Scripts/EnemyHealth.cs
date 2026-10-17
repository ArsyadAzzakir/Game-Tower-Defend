using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 400; 
    [SerializeField] private float healthScaleFactor = 1.3f; 
    
    [Header("Audio Kematian")]
    public AudioClip suaraAsapMati;
    private static float waktuBolehBunyiMati = 0f;

    public GameObject efekAsap;
    public GameObject prefabAngkaDamage;
    
    [Header("Efek Visual Kedip (Hit Flash)")]
    public Color warnaLuka = Color.red;   
    public float durasiKedip = 0.1f;      
    
    private SpriteRenderer[] semuaSprite; 
    private Color[] warnaAsli;            

    [Header("Status Efek: Slow")]
    public Color warnaSlow = new Color(0.5f, 0.8f, 1f, 1f); // Biru es pucat
    private bool isImmuneToSlow = false;
    private bool isSlowed = false;
    
    private EnemyMovement scriptGerak; 

    private void Start()
    {
        semuaSprite = GetComponentsInChildren<SpriteRenderer>();
        
        warnaAsli = new Color[semuaSprite.Length];
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            warnaAsli[i] = semuaSprite[i].color;
        }

        // Ambil komponen EnemyMovement
        scriptGerak = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(int damage, bool isCrit = false)
    {
        hitPoints -= damage;

        GameObject teksDamage = Instantiate(prefabAngkaDamage, transform.position, Quaternion.identity);
        if (teksDamage != null)
        {
            teksDamage.GetComponent<FlyingText>().SetAngka(damage, isCrit);
        }
        
        StartCoroutine(EfekKedipLuka());

        if (hitPoints <= 0)
        {
            if (Random.value <= 0.5f) 
            {
                int goldDrop = Random.Range(1, 7); 
                GameManager.main.AddGold(goldDrop);
            }
            if (suaraAsapMati != null)
            {
                MainkanSuaraKematian(suaraAsapMati, Camera.main.transform.position, 0.2f);
            }

            EnemySpawner.OnEnemyDestroy.Invoke(); 
            Instantiate(efekAsap, transform.position, Quaternion.identity);
            Destroy(gameObject); 
        }
    }

    public void BoostHealt(int waveNumber)
    {
        float scaledHealth = hitPoints * Mathf.Pow(healthScaleFactor, waveNumber - 1);
        hitPoints = Mathf.RoundToInt(scaledHealth) + (waveNumber * 2);
    }  

    private IEnumerator EfekKedipLuka()
    {
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            if (semuaSprite[i] != null) semuaSprite[i].color = warnaLuka;
        }
        
        yield return new WaitForSeconds(durasiKedip);
        
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            if (semuaSprite[i] != null) 
            {
                semuaSprite[i].color = isSlowed ? warnaSlow : warnaAsli[i];
            }
        }
    }
    
    private void MainkanSuaraKematian(AudioClip clip, Vector3 posisi, float volume)
    {
        if (Time.time < waktuBolehBunyiMati) return; 
        waktuBolehBunyiMati = Time.time + 0.05f;

        GameObject speakerTemp = new GameObject("TempAudio_EnemyDeath");
        speakerTemp.transform.position = posisi;

        AudioSource sumberSuara = speakerTemp.AddComponent<AudioSource>();
        sumberSuara.clip = clip;
        sumberSuara.volume = volume;
        sumberSuara.pitch = Random.Range(0.8f, 1.2f);
        sumberSuara.Play();

        Destroy(speakerTemp, clip.length);
    }

    // === SISTEM FROST AURA (SLOW) ===
    public void ApplySlow(float duration)
    {
        if (isImmuneToSlow || scriptGerak == null) return; 
        
        StartCoroutine(SlowRoutine(duration));
    }

    private IEnumerator SlowRoutine(float duration)
    {
        isImmuneToSlow = true;
        isSlowed = true;

        for (int i = 0; i < semuaSprite.Length; i++)
        {
            if (semuaSprite[i] != null) semuaSprite[i].color = warnaSlow;
        }

        // Panggil fungsi perlambatan di EnemyMovement (Dikali 0.7 = Sisa 70% dari kecepatan normal)
        scriptGerak.ApplySlowEffect(0.7f);

        yield return new WaitForSeconds(duration);

        isSlowed = false;
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            if (semuaSprite[i] != null) semuaSprite[i].color = warnaAsli[i];
        }

        // Panggil fungsi normalkan kecepatan kembali
        scriptGerak.RemoveSlowEffect();

        yield return new WaitForSeconds(2f);
        
        isImmuneToSlow = false;
    }
}