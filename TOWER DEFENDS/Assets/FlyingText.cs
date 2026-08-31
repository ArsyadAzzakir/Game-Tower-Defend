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
    public void SetAngka(int nilaiDamage)
    {
        GetComponent<TextMeshPro>().text = "-" + nilaiDamage.ToString();
    }
}
