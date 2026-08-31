using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Harga Pasukan")]
    public int archerPrice = 10;
    public int monkPrice = 20;

    // Fungsi ini akan dipanggil saat tombol Archer ditekan
    public void BuyArcher()
    {
        if (GameManager.main.gold >= archerPrice)
        {
            BuildManager.main.SelectArcher(archerPrice);
            
        }
        else
        {
            Debug.Log("Uang tidak cukup Bos!");
        }
    }

    // Fungsi ini akan dipanggil saat tombol Monk ditekan
    public void BuyMonk()
    {
        if (GameManager.main.gold >= monkPrice)
        {
            BuildManager.main.SelectMonk(monkPrice);
        }
        else
        {
            Debug.Log("Uang tidak cukup Bos!");
        }
    }
}