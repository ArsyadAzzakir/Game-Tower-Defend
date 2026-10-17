using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    [Header("Pengaturan Gerak")]
    public float kecepatan = 0.5f; 
    
    [Header("Batas Layar")]
    public float batasHilangX = 15f;  // Titik awan menghilang di ujung kanan
    public float titikMunculX = -15f; // Titik awan respawn di ujung kiri

    private void Update()
    {
        // Bergerak lambat ke arah kanan secara konstan
        transform.Translate(Vector3.right * kecepatan * Time.deltaTime);

        // Jika awan sudah melewati batas kanan layar, teleportasi kembali ke kiri
        if (transform.position.x > batasHilangX)
        {
            transform.position = new Vector3(titikMunculX, transform.position.y, transform.position.z);
        }
    }
}