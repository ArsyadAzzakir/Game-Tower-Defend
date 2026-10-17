using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Card", menuName = "Drafting/Card")]
public class CardData : ScriptableObject
{
    // Nama dan deskripsi ini sekarang opsional (hanya untuk catatanmu sendiri)
    public string cardName; 
    public Sprite cardIcon; // KITA AKAN MASUKKAN GAMBAR FULL KARTUMU KE SINI
    
    public BuffTarget targetUnit;
    public float buffAmount; 
}

// Tambahkan daftar efek sesuai dengan gambar kartumu
public enum BuffTarget
{
    ArcherDamage,
    ArcherAttackSpeed,
    ArcherCriticalRate,
    ArcherAttackRange,
    

    // === KNIGHT ===
    KnightDeflectDamage,  
    KnightAttackSpeed,    
    KnightDamage,         
    KnightAttackRange,    
    KnightBaseHP,         
    
    // === LANCER ===
    LancerBleedDuration,  
    LancerAttackSpeed,    
    LancerDamage,         

    // === MONK ===
    MonkAoESlow,          
    MonkDamage,
    MonkAttackSpeed            
}