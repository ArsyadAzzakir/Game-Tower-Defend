using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 4; 
    [SerializeField] private float healthScaleFactor = 1.3f; 
    [Header("Audio Kematian")]
    public AudioClip suaraAsapMati;
    private static float waktuBolehBunyiMati = 0f;

    public GameObject efekAsap;
    public GameObject prefabAngkaDamage;
    
    [Header("Efek Visual Kedip (Hit Flash)")]
    public Color warnaLuka = Color.red;   
    public float durasiKedip = 0.1f;      
    
    // Kita buat ini menjadi array (kumpulan) untuk mendeteksi semua bagian tubuh
    private SpriteRenderer[] semuaSprite; 
    private Color[] warnaAsli;            

    private void Start()
    {
        // 1. CARI SEMUA GAMBAR: Skrip otomatis mencari semua SpriteRenderer di musuh ini
        semuaSprite = GetComponentsInChildren<SpriteRenderer>();
        
        // 2. SIMPAN SEMUA WARNA ASLI
        warnaAsli = new Color[semuaSprite.Length];
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            warnaAsli[i] = semuaSprite[i].color;
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;

        GameObject teksDamage = Instantiate(prefabAngkaDamage, transform.position, Quaternion.identity);
        teksDamage.GetComponent<FlyingText>().SetAngka(damage);
        
        // Mulai kedipan sapu jagat
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
            Destroy(gameObject); // Catatan: Jika musuh mati, Coroutine kedip akan otomatis batal, ini aman.
        }
    }

    public void BoostHealt(int waveNumber)
    {
        float scaledHealth = hitPoints * Mathf.Pow(healthScaleFactor, waveNumber - 1);
        hitPoints = Mathf.RoundToInt(scaledHealth) + (waveNumber * 2);
    }  

    private IEnumerator EfekKedipLuka()
    {
        // Ubah SEMUA bagian tubuh jadi merah
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            if (semuaSprite[i] != null) semuaSprite[i].color = warnaLuka;
        }
        
        yield return new WaitForSeconds(durasiKedip);
        
        // Kembalikan SEMUA ke warna semula
        for (int i = 0; i < semuaSprite.Length; i++)
        {
            if (semuaSprite[i] != null) semuaSprite[i].color = warnaAsli[i];
        }
    }
    private void MainkanSuaraKematian(AudioClip clip, Vector3 posisi, float volume)
    {
        if (Time.time < waktuBolehBunyiMati) return; 
        waktuBolehBunyiMati = Time.time + 0.05f;

        GameObject speakerTemp = new GameObject("TempAudio_EnemyDeath");
        speakerTemp.transform.position = posisi;

        // 2. Pasang komponen pemutar suara
        AudioSource sumberSuara = speakerTemp.AddComponent<AudioSource>();
        sumberSuara.clip = clip;
        sumberSuara.volume = volume;
        
        // 3. ACAK NADA! (0.8 hingga 1.2 memberikan variasi dari berat ke ringan)
        sumberSuara.pitch = Random.Range(0.8f, 1.2f);
        
        // 4. Mainkan suaranya
        sumberSuara.Play();

        // 5. Hancurkan "speaker" ini secara otomatis begitu durasi suaranya habis
        Destroy(speakerTemp, clip.length);
    }
}