using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICardDisplay : MonoBehaviour
{
    public Image cardImage;
    public Button cardButton;
    private CardData myData;

    [Header("Pengaturan Efek Mengambang (Aman untuk UI)")]
    public float floatSpeed = 2f;      
    public float scaleAmplitude = 0.03f; 
    public float rotationAmount = 2f;  

    private bool isIdle = false;
    private float randomOffset;

    private void Start()
    {
        randomOffset = Random.Range(0f, 10f);
        StartCoroutine(AnimateCardAppear());
    }

    private void Update()
    {
        if (isIdle)
        {
            float time = Time.unscaledTime * floatSpeed + randomOffset;
            
            float scaleFloat = 1f + Mathf.Sin(time) * scaleAmplitude; 
            transform.localScale = new Vector3(scaleFloat, scaleFloat, 1f); // Z dikunci ke 1f

            float rotZ = Mathf.Cos(time * 0.7f) * rotationAmount; 
            transform.localRotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    public void SetupCard(CardData data)
    {
        myData = data;
        if (cardImage != null && data != null)
        {
            cardImage.sprite = data.cardIcon;
        }

        // === PENYAMBUNG TOMBOL OTOMATIS ===
        if (cardButton != null)
        {
            // 1. Bersihkan sisa "kabel" sebelumnya (mencegah error terpencet 2x)
            cardButton.onClick.RemoveAllListeners();
            
            // 2. Sambungkan tombol ini langsung ke fungsi OnCardChosen di DraftManager
            cardButton.onClick.AddListener(() => DraftManager.main.OnCardChosen(myData));
        }
    }

    private IEnumerator AnimateCardAppear()
    {
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.identity; 

        int urutanKartu = transform.GetSiblingIndex(); 
        yield return new WaitForSecondsRealtime(urutanKartu * 0.15f);

        float duration = 0.4f; 
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; 
            float t = elapsed / duration;

            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            float scale = 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

            transform.localScale = new Vector3(scale, scale, 1f); // Z dikunci ke 1f
            
            yield return null;
        }

        transform.localScale = Vector3.one;
        isIdle = true; 
    }
}