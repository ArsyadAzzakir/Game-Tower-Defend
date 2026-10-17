using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager main;

    [Header("Global Stat Multipliers (Archer)")]
    // Variabel stat dasar (1.0 = 100% / Normal)
    public float archerDamageMultiplier = 1.0f; 
    public float archerAttackSpeedMultiplier = 1.0f;
    public float archerCriticalRateBonus = 0f; // Bonus Crit biasanya mulai dari 0
    public float archerAttackRangeMultiplier = 1.0f;
    
    [Header("Knight Buffs")]
    public float knightDeflectMultiplier = 0f; // Default 0 (Belum ada pantulan)
    public float knightAttackSpeedMultiplier = 1.0f;
    public float knightDamageMultiplier = 1.0f;
    public float knightAttackRangeMultiplier = 1.0f;
    public float knightBaseHPMultiplier = 1.0f;

    [Header("Lancer Buffs")]
    public float lancerBleedDuration = 0f; // Waktu pendarahan
    public float lancerAttackSpeedMultiplier = 1.0f;
    public float lancerDamageMultiplier = 1.0f;

    [Header("Monk Buffs")]
    public float monkAttackSpeedMultiplier = 1.0f;
    public float monkSlowDuration = 0f; // Durasi musuh melambat (Frost Aura)
    public float monkDamageMultiplier = 1.0f;

    private void Awake()
    {
        // Sistem keamanan Singleton agar BuffManager tidak bentrok atau keriset
        if (main == null)
        {
            main = this;
            // Jika suatu saat kamu memisah Main Menu dan Gameplay ke scene berbeda, 
            // hilangkan tanda // pada baris di bawah ini agar data kartu tidak hilang saat pindah scene:
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // Fungsi ini dipanggil saat pemain memilih kartu
    public void ApplyBuff(CardData card)
    {
        switch (card.targetUnit)
        {
            // === ARCHER ===
            case BuffTarget.ArcherDamage:
                archerDamageMultiplier += card.buffAmount; 
                break;
            case BuffTarget.ArcherAttackSpeed:
                archerAttackSpeedMultiplier += card.buffAmount;
                break;
            case BuffTarget.ArcherCriticalRate:
                archerCriticalRateBonus += card.buffAmount;
                break;
            case BuffTarget.ArcherAttackRange:
                archerAttackRangeMultiplier += card.buffAmount;
                break;

            // === KNIGHT ===
            case BuffTarget.KnightDeflectDamage:
                knightDeflectMultiplier += card.buffAmount; 
                break;
            case BuffTarget.KnightAttackSpeed:
                knightAttackSpeedMultiplier += card.buffAmount;
                break;
            case BuffTarget.KnightDamage:
                knightDamageMultiplier += card.buffAmount;
                break;
            case BuffTarget.KnightAttackRange:
                knightAttackRangeMultiplier += card.buffAmount;
                break;
            case BuffTarget.KnightBaseHP:
                knightBaseHPMultiplier += card.buffAmount;
                break;

            // === LANCER ===
            case BuffTarget.LancerBleedDuration:
                lancerBleedDuration += card.buffAmount;
                break;
            case BuffTarget.LancerAttackSpeed:
                lancerAttackSpeedMultiplier += card.buffAmount;
                break;
            case BuffTarget.LancerDamage:
                lancerDamageMultiplier += card.buffAmount;
                break;

            // === MONK ===
            case BuffTarget.MonkAttackSpeed:
                monkAttackSpeedMultiplier += card.buffAmount;
                break;
            case BuffTarget.MonkAoESlow:
                monkSlowDuration += card.buffAmount; // Efek Frost Aura
                break;
            case BuffTarget.MonkDamage:
                monkDamageMultiplier += card.buffAmount; // Efek Wrathful Aura
                break;
        }
        
        Debug.Log($"Buff Diterapkan! {card.targetUnit} bertambah {card.buffAmount}");
    }
}