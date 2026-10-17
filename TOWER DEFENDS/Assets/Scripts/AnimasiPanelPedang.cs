using System.Collections;
using UnityEngine;

public class AnimasiPanelPedang : MonoBehaviour
{
    [Header("Pengaturan UI")]
    public RectTransform panelPedang; // Masukkan Panel Pedang ke sini
    
    [Header("Pengaturan Ukuran")]
    public float lebarAwal = 150f;     // Lebar saat pedang menutup (hanya gagang & ujung)
    public float lebarMaksimal = 800f; // Lebar akhir pedang saat terbuka penuh
    public float durasiAnimasi = 0.5f; // Berapa detik waktu untuk memanjang

    private void OnEnable()
    {
        // Fitur OnEnable memastikan animasi berjalan otomatis setiap kali panel di-SetActivate(true)
        if (panelPedang != null)
        {
            StartCoroutine(PanjangkanPedang());
        }
    }

    private IEnumerator PanjangkanPedang()
    {
        float waktuBerjalan = 0f;
        
        // Simpan ukuran Y agar tingginya tidak berubah, hanya X (lebar) yang dimanipulasi
        Vector2 ukuranAwal = new Vector2(lebarAwal, panelPedang.sizeDelta.y);
        Vector2 ukuranTarget = new Vector2(lebarMaksimal, panelPedang.sizeDelta.y);

        // Reset ukuran ke paling pendek di awal frame
        panelPedang.sizeDelta = ukuranAwal;

        // Looping transisi animasi memanjang
        while (waktuBerjalan < durasiAnimasi)
        {
            waktuBerjalan += Time.deltaTime;
            
            // Mathf.SmoothStep memberikan efek "easing" (lambat di awal dan akhir) agar lebih natural
            float progres = Mathf.SmoothStep(0f, 1f, waktuBerjalan / durasiAnimasi);
            
            panelPedang.sizeDelta = Vector2.Lerp(ukuranAwal, ukuranTarget, progres);
            yield return null;
        }

        // Pastikan ukuran terkunci sempurna di angka maksimal pada akhir animasi
        panelPedang.sizeDelta = ukuranTarget;
    }
}