using UnityEngine;
using DG.Tweening;
/// <summary>
/// プレイヤーの所持アイテム
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("Item Position")]
    public Transform holdPoint;

    public ItemData currentHeldItem;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// アイテムを拾う処理
    /// </summary>
    public void PickUpItem(GameObject itemObject)
    {
        if (currentHeldItem != null) return;

        currentHeldItem = itemObject.GetComponent<ItemData>();
        currentHeldItem.isCaught = true; 

        Collider2D col = itemObject.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = itemObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = false; 
            rb.linearVelocity = Vector2.zero; 
        }

        DreamItemBehavior behavior = itemObject.GetComponent<DreamItemBehavior>();
        if (behavior != null) behavior.enabled = false;

        itemObject.transform.SetParent(holdPoint);
        
        // DOTween
        itemObject.transform.DOLocalMove(Vector3.zero, 0.2f).SetEase(DG.Tweening.Ease.OutBack);
        
        Debug.Log($"{currentHeldItem.currentType} を拾いました！");
    }

    /// <summary>
    /// アイテムを渡す処理
    /// </summary>
    public void DropItem()
    {
        if (currentHeldItem != null)
        {
            Destroy(currentHeldItem.gameObject); // アイテムのオブジェクトを消去
            currentHeldItem = null; // 手を空にする
        }
    }

    /// <summary>
    /// 現在アイテムを持っているか
    /// </summary>
    public bool HasItem()
    {
        return currentHeldItem != null;
    }

    public GameObject PassItem()
    {
        if (currentHeldItem == null) return null;

        GameObject itemToPass = currentHeldItem.gameObject;
        currentHeldItem = null; // 自分の手は空にする
        return itemToPass;
    }
}