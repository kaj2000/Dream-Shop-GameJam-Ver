using UnityEngine;

/// <summary>
/// 商品棚
/// </summary>
public class ShelfSlot : MonoBehaviour
{
    [Header("Item Central")]
    public Transform slotPoint; 

    [Header("Slot Item")]
    public ItemData heldItem;

    public bool IsEmpty()
    {
        return heldItem == null;
    }

    /// <summary>
    /// アイテムをスロットに置く
    /// </summary>
    public void PlaceItem(GameObject itemObj)
    {
        heldItem = itemObj.GetComponent<ItemData>();
        
        heldItem.isCaught = false;
        itemObj.transform.SetParent(slotPoint);
        itemObj.transform.localPosition = Vector3.zero;

    }

    /// <summary>
    /// スロットからアイテムを取り出す
    /// </summary>
    public GameObject TakeItem()
    {
        if (heldItem == null) return null;

        GameObject itemToTake = heldItem.gameObject;
        heldItem = null; // スロットを空にする
        return itemToTake;
    }
}