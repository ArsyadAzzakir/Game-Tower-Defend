using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("Referensi Pabrik Archer")]
    public GameObject archerPrefab; 
    public Sprite archerSprite;     
    
    [Header("Referensi Pabrik Monk")]
    public GameObject monkPrefab; 
    public Sprite monkSprite; 

    [Header("Referensi Pabrik Lancer")]
    public GameObject lancerPrefab; 
    public Sprite lancerSprite; 

    [Header("Referensi Pabrik Knight")]
    public GameObject knightPrefab; 
    public Sprite knightSprite; 
    
    [Header("Sensor Lapak & Tower")]
    public LayerMask towerLayer;      
    public LayerMask normalLapakLayer; // Untuk Archer, Monk, Lancer (Layer Lapak)
    public LayerMask pathLapakLayer;   // Khusus untuk Knight (Layer Path)
    public float checkRadius = 0.5f;

    [Header("Gambar Kursor")]
    public Texture2D cursorDefault; 
    public Texture2D cursorValid;   
    public Texture2D cursorInvalid; 

    private GameObject towerToBuild;
    private int currentPrice;
    private SpriteRenderer hologramRenderer;
    private bool isBuildingKnight = false; // Penanda apakah yang dipegang Knight

    private void Awake()
    {
        main = this;
        hologramRenderer = GetComponent<SpriteRenderer>();
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }

    public void SelectArcher(int price)
    {
        towerToBuild = archerPrefab;
        currentPrice = price;
        hologramRenderer.sprite = archerSprite; 
        isBuildingKnight = false;
    }

    public void SelectMonk(int price)
    {
        towerToBuild = monkPrefab;
        currentPrice = price;
        hologramRenderer.sprite = monkSprite; 
        isBuildingKnight = false;
    }

    public void SelectLancer(int price)
    {
        towerToBuild = lancerPrefab;
        currentPrice = price;
        hologramRenderer.sprite = lancerSprite; 
        isBuildingKnight = false;
    }

    public void SelectKnight(int price)
    {
        towerToBuild = knightPrefab;
        currentPrice = price;
        hologramRenderer.sprite = knightSprite; 
        isBuildingKnight = true; // Menandakan kita sedang memegang Knight!
    }

    private void Update()
    {
        if (towerToBuild != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            // PILIH LAYER LAPAK BERDASARKAN APA YANG DIBANGUN
            // Jika Knight, cek lapak "Path". Jika tower lain, cek lapak "Lapak" biasa.
            LayerMask activeLapakLayer = isBuildingKnight ? pathLapakLayer : normalLapakLayer;

            Collider2D areaSah = Physics2D.OverlapCircle(mousePos, checkRadius, activeLapakLayer);
            bool isLapakValid = (areaSah != null); 
            
            bool isTowerEmpty = (Physics2D.OverlapCircle(mousePos, checkRadius, towerLayer) == null); 
            bool isNotOverUI = !EventSystem.current.IsPointerOverGameObject(); 
            bool isUangCukup = (GameManager.main.gold >= currentPrice); 

            // Syarat mutlak membangun
            bool canBuild = isLapakValid && isTowerEmpty && isNotOverUI && isUangCukup;

            if (canBuild)
            {
                Cursor.SetCursor(cursorValid, new Vector2(cursorValid.width / 2, cursorValid.height / 2), CursorMode.Auto);
                hologramRenderer.color = new Color(1f, 1f, 1f, 0.7f); 
            }
            else
            {
                Cursor.SetCursor(cursorInvalid, new Vector2(cursorInvalid.width / 2, cursorInvalid.height / 2), CursorMode.Auto);
                hologramRenderer.color = new Color(1f, 0f, 0f, 0.7f); 
            }

            // SAAT DIKLIK KIRI
            if (Input.GetMouseButtonDown(0))
            {
                if (canBuild)
                {
                    GameManager.main.AddGold(-currentPrice); 
                    Instantiate(towerToBuild, areaSah.transform.position, Quaternion.identity); 
                    CancelBuilding(); 
                }
                else if (isNotOverUI) 
                {
                    CancelBuilding();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelBuilding();
            }
        }
    }

    private void CancelBuilding()
    {
        towerToBuild = null;
        hologramRenderer.sprite = null; 
        isBuildingKnight = false;
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }
}