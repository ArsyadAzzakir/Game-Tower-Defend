using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake main;

    private void Awake()
    {
        // Memastikan skrip ini bisa dipanggil dari mana saja
        if (main == null) main = this;
    }

    // Fungsi ini yang akan dipanggil oleh ledakan Monk atau saat markas diserang
    public void Shake(float durasi, float kekuatan)
    {
        StartCoroutine(MulaiGetar(durasi, kekuatan));
    }

    private IEnumerator MulaiGetar(float durasi, float kekuatan)
    {
        Vector3 posisiAsli = transform.localPosition;
        float waktuBerjalan = 0f;

        while (waktuBerjalan < durasi)
        {
            float x = Random.Range(-1f, 1f) * kekuatan;
            float y = Random.Range(-1f, 1f) * kekuatan;

            // Menggeser kamera secara acak dengan sangat cepat
            transform.localPosition = new Vector3(posisiAsli.x + x, posisiAsli.y + y, posisiAsli.z);

            waktuBerjalan += Time.deltaTime;
            yield return null; 
        }

        // Kembalikan posisi kamera ke tengah agar tidak miring setelah bergetar
        transform.localPosition = posisiAsli;
    }
}