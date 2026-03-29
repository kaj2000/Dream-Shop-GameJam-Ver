using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening; 

public class QuoteSceneManager : MonoBehaviour
{
    [Header("UI Set")]
    public TextMeshProUGUI quoteText;

    [Header("Time Rythm")]
    public float fadeInTime = 2.0f;  // 淡入需要的时间
    public float waitTime = 3.5f;    // 停留在屏幕上的时间
    public float fadeOutTime = 1.5f; // 淡出需要的时间

    [Header("Next Level")]
    public string nextSceneName = "00a_LevelSelectScene"; // 跳转到选关界面

    void Start()
    {
        if (quoteText != null)
        {
            quoteText.color = new Color(quoteText.color.r, quoteText.color.g, quoteText.color.b, 0);


            Sequence seq = DOTween.Sequence();
            

            seq.Append(quoteText.DOFade(1f, fadeInTime).SetEase(Ease.InOutQuad));
            

            seq.AppendInterval(waitTime);
            

            seq.Append(quoteText.DOFade(0f, fadeOutTime).SetEase(Ease.InOutQuad));
            

            seq.OnComplete(() =>
            {
                SceneManager.LoadScene(nextSceneName);
            });
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}