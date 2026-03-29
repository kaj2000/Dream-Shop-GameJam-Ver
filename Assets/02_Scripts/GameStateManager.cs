using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; 
using TMPro; 
using UnityEngine.SceneManagement; 

/// <summary>
/// ゲーム全体管理
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

 
    public enum GamePhase { Starting, Prep, Normal, Chaos, GameOver }
    public GamePhase currentPhase = GamePhase.Starting;

    [Header("Total Time")]
    public float totalTime = 180f; 
    private float timer = 0f;

    [Header("Step rate")]
    public float prepRatio = 0.25f;   
    public float normalRatio = 0.20f; 

    [Header("Dream Clock")]
    public Image phaseFillImage; 
    public RectTransform clockHand; 

    [Header("Central Text")]
    public TextMeshProUGUI announcementText; 
    public CanvasGroup announcementCanvasGroup; 


    [Header("White FlashUI")]
    public Image flashImage; 

    [Header("Result UI")]
    public GameObject resultPanel;          
    public TextMeshProUGUI finalScoreText;  

    private float prepEndTime;
    private float normalEndTime;

    private bool prepTriggered = false;
    private bool normalTriggered = false;
    private bool chaosTriggered = false;

    private int lastCountdownSecond = -1; 

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        prepEndTime = totalTime * prepRatio;
        normalEndTime = prepEndTime + (totalTime * normalRatio);

        if (phaseFillImage != null) phaseFillImage.fillAmount = 0f;
        if (resultPanel != null) resultPanel.SetActive(false);

        currentPhase = GamePhase.Prep;
        timer = 0f;
        
        OrderManager.Instance.SetShopState(false); 
        AudioManager.Instance.PlayBGM(AudioManager.Instance.normalBGM);
        ShowAnnouncement("開店準備中...", new Color(0.2f, 0.8f, 1f)); 
    }

    void Update()
    {
        if (currentPhase == GamePhase.GameOver) return;

        timer += Time.deltaTime;

        UpdateClockUI();

        float timeLeft = totalTime - timer;
        if (timeLeft <= 5.0f && timeLeft > 0f)
        {
            int secondsLeft = Mathf.CeilToInt(timeLeft); 
            
            if (secondsLeft != lastCountdownSecond)
            {
                lastCountdownSecond = secondsLeft;
                PlayCountdownAnim(secondsLeft.ToString());
            }
        }

        CheckPhaseTransition();
    }

    void UpdateClockUI()
    {
        if (phaseFillImage != null && clockHand != null)
        {
            float progress = timer / totalTime;
            phaseFillImage.fillAmount = progress;
            float angle = progress * 360f;
            clockHand.localEulerAngles = new Vector3(0, 0, -angle); 
        }
    }

    void CheckPhaseTransition()
    {
        if (timer < prepEndTime && !prepTriggered)
        {
            prepTriggered = true;
        }
        else if (timer >= prepEndTime && timer < normalEndTime && !normalTriggered)
        {
            normalTriggered = true;
            currentPhase = GamePhase.Normal;
            OrderManager.Instance.SetShopState(true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.shopOpenSound);
            ShowAnnouncement("営業開始！", new Color(1f, 0.6f, 0f)); 
        }
        else if (timer >= normalEndTime && timer < totalTime && !chaosTriggered)
        {
            chaosTriggered = true;
            currentPhase = GamePhase.Chaos;
            AudioManager.Instance.PlayBGM(AudioManager.Instance.chaosBGM);
            ChaosManager.Instance.ActivateUraPhase();
            ShowAnnouncement("ーー裏営業、開始ーー", new Color(0.8f, 0f, 0.4f)); 
        }
        else if (timer >= totalTime)
        {
            GameOver();
        }
    }


    void GameOver()
    {
        currentPhase = GamePhase.GameOver;
        Debug.Log("营业终了！");
        OrderManager.Instance.TriggerGameOver("営業終了！");

        int finalScore = OrderManager.Instance.score;

        int nextLevel = LevelSelectManager.currentPlayingLevel + 1;
        if (nextLevel > LevelSelectManager.maxUnlockedLevel && nextLevel <= 3)
        {
            LevelSelectManager.maxUnlockedLevel = nextLevel;
        }


        if (flashImage != null && resultPanel != null)
        {

            flashImage.gameObject.SetActive(true);
            flashImage.color = new Color(1, 1, 1, 0);


            flashImage.DOFade(1f, 0.15f).OnComplete(() =>
            {

                resultPanel.SetActive(true);
                if (finalScoreText != null) finalScoreText.text = finalScore.ToString();

  
                flashImage.DOFade(0f, 0.5f).OnComplete(() => 
                {
                    flashImage.gameObject.SetActive(false);
                });
            });
        }
        else
        {

            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
                if (finalScoreText != null) finalScoreText.text = finalScore.ToString(); 
            }
        }
    }

    public void PlayCountdownAnim(string numberText)
    {
        if (announcementText == null || announcementCanvasGroup == null) return;
        announcementCanvasGroup.DOKill();
        announcementText.transform.DOKill();
        announcementText.text = numberText;
        announcementText.color = Color.red; 
        announcementText.gameObject.SetActive(true);
        announcementCanvasGroup.alpha = 1f;
        announcementText.transform.localScale = Vector3.one * 1.5f;
        announcementText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);
        announcementCanvasGroup.DOFade(0f, 0.8f).SetDelay(0.2f);
    }

    public void ShowAnnouncement(string message, Color textColor)
    {
        if (announcementText == null || announcementCanvasGroup == null) return;
        announcementText.text = message;
        announcementText.color = textColor;
        announcementCanvasGroup.alpha = 0f;
        announcementText.transform.localScale = Vector3.one * 0.5f; 
        announcementText.gameObject.SetActive(true);
        announcementCanvasGroup.DOKill();
        announcementText.transform.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Append(announcementCanvasGroup.DOFade(1f, 0.4f));
        seq.Join(announcementText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack));
        seq.AppendInterval(1.5f);
        seq.Append(announcementCanvasGroup.DOFade(0f, 0.5f));
        seq.Join(announcementText.transform.DOScale(1.3f, 0.5f).SetEase(Ease.InQuad));
        seq.OnComplete(() => announcementText.gameObject.SetActive(false));
    }
    
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("01_LevelSelect");
    }

    public void ShareToTwitter()
    {
        int finalScore = OrderManager.Instance.score;
        int level = LevelSelectManager.currentPlayingLevel;
        string text = $"『夢売店』STAGE {level} でスコア {finalScore} を獲得した！ウラ世界のコンビニバイト、過酷すぎる… #unity1week #夢売店";
        string url = "https://twitter.com/intent/tweet?text=" + UnityEngine.Networking.UnityWebRequest.EscapeURL(text);
        Application.OpenURL(url);
    }
}