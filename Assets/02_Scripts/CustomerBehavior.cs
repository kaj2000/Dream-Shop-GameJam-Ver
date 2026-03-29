using UnityEngine;
using UnityEngine.UI; // Image（UI）操作用

/// <summary>
/// 顧客の忍耐力（タイマー）と怒り状態を管理するクラス
/// </summary>
public class CustomerBehavior : MonoBehaviour
{
    [Header("忍耐力設定")]
    public float maxPatience = 15f; // 15秒で怒って帰る
    private float currentPatience;

    [Header("UI設定")]
    public Image patienceFill; 

    private bool isWaiting = false;

    void Start()
    {
        // ★ 変更：店に入って列に並んだ瞬間から忍耐力が減り始める！
        currentPatience = maxPatience;
        isWaiting = true;
    }

    void Update()
    {
        if (!isWaiting) return;

        // 毎フレーム忍耐力を減らす
        currentPatience -= Time.deltaTime;

        // UIバーの填充量を更新 (0.0 ～ 1.0)
        if (patienceFill != null)
        {
            patienceFill.fillAmount = currentPatience / maxPatience;
        }

        // 忍耐力が尽きたら
        if (currentPatience <= 0)
        {
            TimeOut();
        }
    }

    /// <summary>
    /// カウンターの先頭に到達し、注文を開始した時
    /// </summary>
    public void StartWaiting()
    {
        // ★ 変更：先頭にたどり着いた安堵感として、忍耐力を3秒回復する！
        currentPatience += 3f;
        if (currentPatience > maxPatience) currentPatience = maxPatience;
    }

    /// <summary>
    /// ★ 追加：商品を間違えられた時のペナルティ
    /// </summary>
    public void ReducePatience(float penalty)
    {
        currentPatience -= penalty;
        if (currentPatience <= 0) TimeOut();
    }

    /// <summary>
    /// タイムアウト（怒って帰る）処理
    /// </summary>
    void TimeOut()
    {
        isWaiting = false;
        Debug.Log("❌ 顧客：遅すぎる！もう帰る！");
        
        // ★ 追加：現在注文中の客か、列に並んでいる客かで処理を分ける
        if (OrderManager.Instance.currentCustomer == this)
        {
            // 先頭の客が怒った場合
            OrderManager.Instance.CustomerTimeOut();
        }
        else
        {
            // 列に並んでいる客が怒った場合（列から離脱して詰める）
            LevelQueueManager.Instance.OnCustomerLeave(this);
        }
    }
}