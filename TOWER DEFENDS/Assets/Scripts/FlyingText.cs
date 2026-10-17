using UnityEngine;
using TMPro;

public class FlyingText : MonoBehaviour
{
    void Start()
    {
        // Teks hancur otomatis setelah 1 detik
        Destroy(gameObject, 1f); 
    }

    void Update()
    {
        // Teks bergerak perlahan ke atas setiap frame
        transform.Translate(Vector3.up * 2f * Time.deltaTime); 
    }

    // Tambahkan parameter isCrit untuk menerima status serangan
    public void SetAngka(int nilaiDamage, bool isCrit)
    {
        TextMeshPro textMesh = GetComponent<TextMeshPro>();
        textMesh.text = "-" + nilaiDamage.ToString();

        if (isCrit)
        {
            // Jika Critical: Ukuran membesar 1.5x dan teks berwarna kuning
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            textMesh.color = Color.red; 
        }
        else
        {
            // Jika Normal: Ukuran standar dan teks berwarna putih
            transform.localScale = new Vector3(1f, 1f, 1f);
            textMesh.color = Color.orange; 
        }
    }
}