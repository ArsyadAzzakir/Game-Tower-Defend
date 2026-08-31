using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager main; // Agar mudah diakses oleh skrip lain

    [Header("Player Stats")]
    public int gold = 50;
    public int baseHealth = 20;
    public int maxBaseHealth = 20;

    [Header("UI References")]
    public TextMeshProUGUI goldText;       
    public TextMeshProUGUI goldTextShadow;
    public Image healthBarFill;

    private void Awake()
    {
        // Memastikan hanya ada satu GameManager di arena
        if (main == null) main = this;
        else Destroy(gameObject);
    }

    private void Start()
    {

        UpdateGoldUI();
        UpdateHealthUI();
    }


    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Dapat Gold: " + amount + " | Total Saldo: " + gold);
        UpdateGoldUI();
    }

    
    public void BaseTakeDamage(int damage)
    {
        baseHealth -= damage;
        Debug.Log("Markas Diserang! Sisa Nyawa: " + baseHealth);
        UpdateHealthUI();
        // Memanggil getaran layar selama 0.15 detik dengan kekuatan 0.1
if (CameraShake.main != null)
{
    CameraShake.main.Shake(0.15f, 0.1f); 
}
        if (baseHealth <= 0)
        {
            Debug.Log("GAME OVER!");
            if (GameOverScreen.main != null)
            {
                GameOverScreen.main.MunculkanGameOver();
            }
        }
    }

    private void UpdateGoldUI()
    {
        if (goldText != null) 
            goldText.text = gold.ToString();
            
        if (goldTextShadow != null) 
            goldTextShadow.text = gold.ToString();
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
        healthBarFill.fillAmount = (float)baseHealth / maxBaseHealth;
        }
    }
}