using UnityEngine;


public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        // Hancurkan objek ini 0.5 detik setelah muncul
        Destroy(gameObject, 0.5f); 
    }
}