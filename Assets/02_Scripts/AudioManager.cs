using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM")]
    public AudioClip titleBGM;   // 标题与选关
    public AudioClip normalBGM;  // 正常营业
    public AudioClip chaosBGM;   // 梦境时刻

    [Header("SFX")]
    public AudioClip shopOpenSound;   // 便利店进门音
    public AudioClip submitItemSound; // 交接商品成功音

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- BGM  ---
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource.clip == clip) return; 
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // --- SFX ---
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip); 
    }
}