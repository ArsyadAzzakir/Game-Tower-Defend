using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk fitur pindah Scene (Keluar Menu)

public class PauseManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public GameObject pausePanel;

    // Variabel pengingat apakah game sedang jeda atau tidak
    public static bool isPaused = false; 

    private void Start()
    {
        // Pastikan game berjalan normal saat baru dimulai
        pausePanel.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
    }

    private void Update()
    {
        // Tekan tombol ESC di keyboard untuk Buka/Tutup menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Fungsi ini bisa dipanggil oleh tombol Resume atau tombol ESC
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Waktu kembali berjalan normal (1x kecepatan)
        isPaused = false;
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Sihir menghentikan waktu! (0x kecepatan)
        isPaused = true;
    }

    // Fungsi ini untuk tombol Quit
    public void QuitToMenu()
    {
        // Wajib mengembalikan waktu ke normal sebelum pindah layar
        Time.timeScale = 1f; 
        isPaused = false;
        
        // Memuat layar Main Menu (nanti kita buat namanya persis seperti ini)
        SceneManager.LoadScene("MainMenu"); 
    }
}