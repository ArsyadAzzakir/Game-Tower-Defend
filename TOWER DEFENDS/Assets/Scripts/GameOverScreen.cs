using UnityEngine;
using UnityEngine.SceneManagement; // Wajib dipanggil untuk merestart level

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen main;
    public GameObject gameOverUI;

    private void Awake()
    {
        main = this;
        gameOverUI.SetActive(false); // Pastikan UI mati saat game baru mulai
    }

    public void MunculkanGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // Membekukan waktu (musuh berhenti jalan)
    }

    public void UlangiLevel()
    {
        Time.timeScale = 1f; // Cairkan waktu kembali sebelum restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}