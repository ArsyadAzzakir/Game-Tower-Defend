using UnityEngine;

public class DustSpawner : MonoBehaviour
{
    public GameObject debuPrefab;
    public float jarakWaktuMuncul = 0.4f; // Makin kecil, makin rapat jejaknya

    void Start()
    {
        // Mulai mencetak debu berulang-ulang saat musuh hidup
        InvokeRepeating("TinggalkanDebu", 0.2f, jarakWaktuMuncul);
    }

    void TinggalkanDebu()
    {
        // Posisikan debu sedikit di bawah titik tengah musuh (area kaki)
        Vector3 posisiKaki = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
        Instantiate(debuPrefab, posisiKaki, Quaternion.identity);
    }
}