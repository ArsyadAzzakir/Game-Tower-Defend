using UnityEngine;
using TMPro;
using System.Collections; 

public class WaveNotifier : MonoBehaviour
{
    public static WaveNotifier main;

    [Header("UI Referensi")]
    public GameObject bannerObject;   
    public TextMeshProUGUI waveText;  
    
    [Header("Pengaturan Animasi")]
    public float tampilBerapaDetik = 2.5f; // Lama diam di layar
    public float kecepatanSlide = 2f;      // Kecepatan meluncur

    private RectTransform bannerRect;
    private CanvasGroup canvasGroup;
    private Vector2 posisiTengah;
    private Vector2 posisiAtas;

    private void Awake()
    {
        main = this;
        
        bannerRect = bannerObject.GetComponent<RectTransform>();
        
        // JURUS 1: Otomatis tambahkan efek transparan biar aman
        canvasGroup = bannerObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = bannerObject.AddComponent<CanvasGroup>();

        // Simpan posisi yang udah kamu atur rapi di Unity
        posisiTengah = bannerRect.anchoredPosition;
        
        // JURUS 2: Paksa tarik pita ke atas sejauh 800 pixel dari posisinya!
        posisiAtas = new Vector2(posisiTengah.x, posisiTengah.y + 800f);
        
        // Sembunyikan di awal
        bannerRect.anchoredPosition = posisiAtas;
        canvasGroup.alpha = 0f; 
        bannerObject.SetActive(false); 
    }

    public void TampilkanNotif(int angkaWave)
    {
        StopAllCoroutines(); 
        StartCoroutine(MainkanAnimasi(angkaWave));
    }

    private IEnumerator MainkanAnimasi(int angkaWave)
    {
        waveText.text = "WAVE " + angkaWave;
        
        // Reset ke atas dan transparan
        bannerRect.anchoredPosition = posisiAtas;
        canvasGroup.alpha = 0f;
        bannerObject.SetActive(true);

        // --- 1. MELUNCUR TURUN (Sambil memudar jelas) ---
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * kecepatanSlide;
            float kurva = Mathf.SmoothStep(0, 1, progress);
            
            bannerRect.anchoredPosition = Vector2.Lerp(posisiAtas, posisiTengah, kurva);
            canvasGroup.alpha = kurva; // Transparan -> Jelas
            
            yield return null; 
        }
        
        // Pastikan pas di tempat
        bannerRect.anchoredPosition = posisiTengah;
        canvasGroup.alpha = 1f;

        // --- 2. DIAM (Berhenti untuk dibaca) ---
        yield return new WaitForSeconds(tampilBerapaDetik);

        // --- 3. MELUNCUR NAIK (Sambil memudar hilang) ---
        progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * kecepatanSlide;
            float kurva = Mathf.SmoothStep(0, 1, progress);
            
            bannerRect.anchoredPosition = Vector2.Lerp(posisiTengah, posisiAtas, kurva);
            canvasGroup.alpha = 1f - kurva; // Jelas -> Transparan
            
            yield return null;
        }

        // --- MATIKAN TOTAL ---
        bannerRect.anchoredPosition = posisiAtas;
        canvasGroup.alpha = 0f;
        bannerObject.SetActive(false);
    }
}