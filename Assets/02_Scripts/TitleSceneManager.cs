using UnityEngine;
using UnityEngine.SceneManagement; 

/// <summary>
/// タイトル画面
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    [Header("Scene Name")]
    public string gameSceneName = "00_QuoteScene";


    private bool isLoading = false; 
    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.titleBGM);
    }
    void Update()
    {

        if (!isLoading && Input.anyKeyDown)
        {
            isLoading = true; // ロックをかける
            Debug.Log("営業開始！");
            

            SceneManager.LoadScene(gameSceneName);
        }
    }
}