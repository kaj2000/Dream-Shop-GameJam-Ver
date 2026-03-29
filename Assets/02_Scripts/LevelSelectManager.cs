using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージ選択画面
/// </summary>
public class LevelSelectManager : MonoBehaviour
{
    [Header("BG Image")]
    public Image backgroundImage;

    [Header("Image Pair")]
    public Sprite[] progress1Sprites = new Sprite[2];
    public Sprite[] progress2Sprites = new Sprite[2];
    public Sprite[] progress3Sprites = new Sprite[2];

    [Header("Flash Speed")]
    public float flickerSpeed = 0.5f;

    [Header("Stage Button")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;


    public static int maxUnlockedLevel = 1;    // 現在どこまで解放されているか
    public static int currentPlayingLevel = 1; // 今から遊ぶステージはどれか

    private float timer = 0f;
    private int currentSpriteIndex = 0;
    private Sprite[] currentActivePair;

    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.titleBGM);
        SetupLevelSelect();
    }

    /// <summary>
    /// 進行度に合わせて背景
    /// </summary>
    void SetupLevelSelect()
    {
        // 進行度をチェックして画像とボタンの状態を決定
        if (maxUnlockedLevel >= 3)
        {
            currentActivePair = progress3Sprites;
            level1Button.interactable = true;
            level2Button.interactable = true;
            level3Button.interactable = true;
        }
        else if (maxUnlockedLevel == 2)
        {
            currentActivePair = progress2Sprites;
            level1Button.interactable = true;
            level2Button.interactable = true;
            level3Button.interactable = false; // 3面ロック
        }
        else
        {
            currentActivePair = progress1Sprites;
            level1Button.interactable = true;
            level2Button.interactable = false; // 2面ロック
            level3Button.interactable = false; // 3面ロック
        }

        // 最初の1枚目を表示
        if (currentActivePair != null && currentActivePair.Length > 0)
        {
            backgroundImage.sprite = currentActivePair[0];
        }
    }

    void Update()
    {
       
        if (currentActivePair == null || currentActivePair.Length < 2) return;

        timer += Time.deltaTime;
        if (timer >= flickerSpeed)
        {
            timer = 0f;
         
            currentSpriteIndex = (currentSpriteIndex == 0) ? 1 : 0; 
            backgroundImage.sprite = currentActivePair[currentSpriteIndex];
        }
    }

       public void OnLevelClicked(int levelNumber)
    {
        Debug.Log($"Level {levelNumber} に出発");
        
        // どのステージ記憶
        currentPlayingLevel = levelNumber; 
        
        // ゲーム本編へ遷移
        SceneManager.LoadScene("0" + levelNumber + "_GameScene");
    }
}