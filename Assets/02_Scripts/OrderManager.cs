using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("UI Set")]
    public TextMeshProUGUI customerOrderText; 
    public TextMeshProUGUI scoreText;

    [Header("Customer Set")]
    public CustomerBehavior currentCustomer;

    [Header("Game Set")]
    public int score = 0; 
    private bool isGameOver = false;

    private ItemData.ItemType targetItemType;

    // ==========================================
    // 辞書 人間用
    // ==========================================
    private Dictionary<ItemData.ItemType, string> normalItemNames = new Dictionary<ItemData.ItemType, string>()
    {
        { ItemData.ItemType.Onigiri, "おにぎり" },
        { ItemData.ItemType.Cola, "コーラ" },
        { ItemData.ItemType.CupNoodle, "カップ麺" } 
    };

    // ==========================================
    // 辞書 裏世界用
    // ==========================================
    private Dictionary<ItemData.ItemType, string[]> dreamItemNames = new Dictionary<ItemData.ItemType, string[]>()
    {
        { ItemData.ItemType.Onigiri, new string[] { 
            "白色の三角形の記憶", 
            "黒き衣を纏いし白銀の山", 
            "塩と米の錬金術" 
        }},  
        { ItemData.ItemType.Cola, new string[] { 
            "冷たい赤色の溜息", 
            "黒き泡立つ霊薬", 
            "眠気を喰らう黒い雫" 
        }},      
        { ItemData.ItemType.CupNoodle, new string[] { 
            "三分間の走馬灯", 
            "熱湯を待つ黄金の糸", 
            "深夜の背徳的魔法" 
        }}        
    };
    // ==========================================

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
        GenerateNewOrder(); 
    }
    
    private bool isShopOpen = false;

    public void SetShopState(bool open)
    {
        isShopOpen = open;
    }

    public void GenerateNewOrder(ItemData itemToRemove = null)
    {
        if (isGameOver) return;

        List<ItemData.ItemType> todayMenu = new List<ItemData.ItemType>();
        
        todayMenu.Add(ItemData.ItemType.Onigiri);

        if (LevelSelectManager.currentPlayingLevel >= 2)
        {
            todayMenu.Add(ItemData.ItemType.Cola);
        }

        if (LevelSelectManager.currentPlayingLevel >= 3)
        {
            todayMenu.Add(ItemData.ItemType.CupNoodle);
        }

        int randomIndex = Random.Range(0, todayMenu.Count);
        targetItemType = todayMenu[randomIndex];

        if (GameStateManager.Instance != null && GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Chaos)
        {
           
            string[] possibleDreamTexts = dreamItemNames[targetItemType]; 
            string selectedDreamText = possibleDreamTexts[Random.Range(0, possibleDreamTexts.Length)];
            
            if (customerOrderText != null) customerOrderText.text = $"「{selectedDreamText} を頂戴」";
        }
        else
        {
            string normalItemText = normalItemNames[targetItemType];
            if (customerOrderText != null) customerOrderText.text = $"「{normalItemText} をください」";
        }

        if (currentCustomer != null)
        {
            currentCustomer.StartWaiting();
        }
    }

    public void CheckItem(ItemData caughtItem)
    {
        if (isGameOver) return;

        if (caughtItem.currentType == targetItemType)
        {
            Debug.Log("正解！");
            score += 100;
            if (AudioManager.Instance != null && AudioManager.Instance.submitItemSound != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.submitItemSound);
            }
            UpdateScoreUI();

            if (currentCustomer != null)
            {
                LevelQueueManager.Instance.OnCustomerLeave(currentCustomer);
            }
        }
        else
        {
            Debug.Log("違う！");
            score -= 30;

            if (currentCustomer != null)
            {
                currentCustomer.ReducePatience(4f);
            }

            UpdateScoreUI();
        }
    }
    
    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {score}";
    }

    public void TriggerGameOver(string message)
    {
        isGameOver = true;
        if (customerOrderText != null) customerOrderText.text = message;
        Debug.Log($"ゲームオーバー: 最終スコア {score}");
    }

    public void CustomerTimeOut()
    {
        if (isGameOver) return;
        score -= 50; 
        UpdateScoreUI();

        if (currentCustomer != null)
        {
            LevelQueueManager.Instance.OnCustomerLeave(currentCustomer);
        }
    }

    public void SetTargetCustomer(CustomerBehavior customer)
    {
        currentCustomer = customer;
        if (currentCustomer == null)
        {
            if (customerOrderText != null) customerOrderText.text = "";
            return;
        }
        customerOrderText = currentCustomer.transform.Find("Canvas/OrderText").GetComponent<TextMeshProUGUI>();
        GenerateNewOrder();
    }
}