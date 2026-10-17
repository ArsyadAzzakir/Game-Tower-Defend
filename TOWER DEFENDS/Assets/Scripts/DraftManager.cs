using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraftManager : MonoBehaviour
{
    public static DraftManager main;

    [Header("UI References")]
    public GameObject draftPanel; // Panel UI yang menutupi layar
    public Transform cardContainer; // Tempat kartu akan di-spawn (biasanya pakai Horizontal Layout Group)
    public GameObject draftCardPrefab; // Prefab UI Kartu
    public Button rerollButton;

    [Header("Drafting Settings")]
    public List<CardData> allAvailableCards; // Masukkan 5 CardData-mu dari folder project ke sini
    public int rerollCost = 50;

    private int rerollCount = 1;

    private void Awake()
    {
        main = this;
        draftPanel.SetActive(false);
    }

    private void Update()
{
    // Tekan tombol T di keyboard untuk memunculkan Draft UI kapan saja! (Khusus Testing)
    if (Input.GetKeyDown(KeyCode.T))
    {
        StartDraftPhase();
    }
}

    // Dipanggil oleh sistem Wave saat Wave % 5 == 0 dan musuh habis
    public void StartDraftPhase()
    {
        Time.timeScale = 0f; // Jeda waktu permainan
        draftPanel.SetActive(true);
        rerollCount = 1;
        UpdateRerollUI();
        RollCards();
    }

    public void RollCards()
    {
        // 1. Bersihkan sisa kartu sebelumnya dari layar
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Buat Copy dari daftar kartu untuk diacak agar tidak ada duplikat
        List<CardData> cardPool = new List<CardData>(allAvailableCards);
        
        // 3. Ambil 3 kartu secara acak (RNG)
        for (int i = 0; i < 3; i++)
        {
            if (cardPool.Count == 0) break; // Jaga-jaga jika kartu kurang dari 3

            int randomIndex = Random.Range(0, cardPool.Count);
            CardData selectedCard = cardPool[randomIndex];
            
            // 4. Hapus dari pool sementara agar tidak terambil 2x (Anti Duplikat)
            cardPool.RemoveAt(randomIndex);

            // 5. Spawn Prefab UI Kartu dan berikan datanya
            GameObject newCard = Instantiate(draftCardPrefab, cardContainer);
            newCard.GetComponent<UICardDisplay>().SetupCard(selectedCard);
        }
    }

    public void OnRerollClicked()
    {
        // Cek syarat mutlak: Punya jatah reroll & uang cukup
        if (rerollCount > 0 && GameManager.main.gold >= rerollCost)
        {
            GameManager.main.AddGold(-rerollCost); // Kurangi koin (Sesuaikan dengan script GameManager-mu)
            rerollCount = 0; // Kunci tombol
            UpdateRerollUI();
            RollCards(); // Acak 3 kartu baru
        }
    }

    private void UpdateRerollUI()
    {
        // Matikan tombol jika jatah habis atau uang tidak cukup
        bool canReroll = (rerollCount > 0 && GameManager.main.gold >= rerollCost);
        rerollButton.interactable = canReroll;
    }

    public void OnCardChosen(CardData chosenCard)
    {
        // Terapkan buff ke Global Modifier
        BuffManager.main.ApplyBuff(chosenCard);

        // Tutup UI dan jalankan waktu kembali
        draftPanel.SetActive(false);
        Time.timeScale = 1f;

        // BISA TAMBAHKAN: Perintah untuk memulai wave berikutnya di sini
    }
}