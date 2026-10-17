using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TombolPedangShop : MonoBehaviour
{
    [Header("Pengaturan UI")]
    public RectTransform panelPedang;
    public GameObject kontenShop; // Masukkan panel berisi teks/item shop di sini

    [Header("Pengaturan Ukuran")]
    public float lebarPendek = 150f;
    public float lebarPanjang = 800f;
    public float durasiAnimasi = 0.4f;

    private bool isBuka = false;
    private Coroutine animasiAktif;

    public void TekanTombolShop()
    {
        isBuka = !isBuka; // Tukar status (buka/tutup)
        
        if (animasiAktif != null) StopCoroutine(animasiAktif);
        animasiAktif = StartCoroutine(AnimasiPedang(isBuka));
    }

    private IEnumerator AnimasiPedang(bool buka)
    {
        // 1. Jika mau TUTUP, hilangkan dulu isi shop-nya agar tidak melayang di udara
        if (!buka && kontenShop != null) kontenShop.SetActive(false);

        float waktuBerjalan = 0f;
        Vector2 ukuranSekarang = panelPedang.sizeDelta;
        Vector2 ukuranTarget = new Vector2(buka ? lebarPanjang : lebarPendek, panelPedang.sizeDelta.y);

        // 2. Animasi memanjang/menyusut
        while (waktuBerjalan < durasiAnimasi)
        {
            waktuBerjalan += Time.deltaTime;
            float progres = Mathf.SmoothStep(0f, 1f, waktuBerjalan / durasiAnimasi);
            panelPedang.sizeDelta = Vector2.Lerp(ukuranSekarang, ukuranTarget, progres);
            yield return null;
        }

        panelPedang.sizeDelta = ukuranTarget;

        // 3. Jika BUKA, munculkan isi shop SETELAH pedang selesai memanjang
        if (buka && kontenShop != null) kontenShop.SetActive(true);
    }
}