using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Harga Pasukan")]
    public int archerPrice = 10;
    public int monkPrice = 20;
    public int lancerPrice = 30; 
    public int knightPrice = 50;

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

    public void BuyLancer()
    {
        if (GameManager.main.gold >= lancerPrice)
        {
            BuildManager.main.SelectLancer(lancerPrice);
            
        }
        else
        {
            Debug.Log("Uang tidak cukup Bos!");
        }
    }

    public void buyKnight()
    {
        if (GameManager.main.gold >= knightPrice)
        {
            BuildManager.main.SelectKnight(knightPrice);

        }
        else
        {
                Debug.Log("Uang tidak cukup Bos!");
        }
    }
}