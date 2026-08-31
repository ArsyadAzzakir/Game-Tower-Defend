using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    void Start()
    {
        // Menghancurkan efek ini setelah 0.5 detik (sesuaikan dengan lama animasimu)
        Destroy(gameObject, 0.5f); 
    }
}