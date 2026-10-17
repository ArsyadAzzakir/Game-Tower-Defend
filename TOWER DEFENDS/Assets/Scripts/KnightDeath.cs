using UnityEngine;

public class KnightDeath : MonoBehaviour
{
    void Start()
    {
        // Menghancurkan efek ini setelah 0.5 detik (sesuaikan dengan lama animasimu)
        Destroy(gameObject, 1.5f); 
    }
}