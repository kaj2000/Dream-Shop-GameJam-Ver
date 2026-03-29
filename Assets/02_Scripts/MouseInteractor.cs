using UnityEngine;
using DG.Tweening; 

/// <summary>
/// マウスを使ったアイテムのクリック
/// </summary>
public class MouseInteractor : MonoBehaviour
{
    [Header("Distance Limit")]
    public float interactRange = 3f;

    [Header("Layer Set")]
    public LayerMask interactableLayer;

    void Update()
    {
        // Left
        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }

        // Right
        if (Input.GetMouseButtonDown(1))
        {
            if (PlayerInventory.Instance.HasItem())
            {
                Debug.Log("拿错了！");
                PlayerInventory.Instance.DropItem(); 
            }
        }
    }

    void TryInteract()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;

        if (Vector2.Distance(playerPosition, mouseWorldPosition) <= interactRange)
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPosition, interactableLayer);

            if (hit != null)
            {

                ItemBox box = hit.gameObject.GetComponent<ItemBox>();
                if (box != null)
                {
                    if (!PlayerInventory.Instance.HasItem()) // 手が空いている時だけ
                    {
                        GameObject newItem = box.TakeNewItem();
                        if (newItem != null) 
                        {
                            PlayerInventory.Instance.PickUpItem(newItem);
                        }
                    }
                    else
                    {
                        Debug.Log("両手が塞がっている！");
                    }
                    return; // 処理終了
                }


                ShelfSlot slot = hit.gameObject.GetComponent<ShelfSlot>();
                if (slot != null)
                {
                    if (PlayerInventory.Instance.HasItem() && slot.IsEmpty())
                    {
                        //手にアイテムがあって、置く
                        GameObject itemToPlace = PlayerInventory.Instance.PassItem();
                        slot.PlaceItem(itemToPlace);
                    }
                    else if (!PlayerInventory.Instance.HasItem() && !slot.IsEmpty())
                    {
                        // 手が空で、 取る
                        GameObject itemToTake = slot.TakeItem();
                        PlayerInventory.Instance.PickUpItem(itemToTake);
                    }
                    return; // 処理終了
                }


                CustomerBehavior customer = hit.gameObject.GetComponent<CustomerBehavior>();
                if (customer != null)
                {
                    if (PlayerInventory.Instance.HasItem())
                    {
                        OrderManager.Instance.CheckItem(PlayerInventory.Instance.currentHeldItem);
                        PlayerInventory.Instance.DropItem(); // 渡したら手元のアイテムは消滅
                    }
                    return;
                }

 
                ItemData itemData = hit.gameObject.GetComponent<ItemData>();
                if (itemData != null && !PlayerInventory.Instance.HasItem())
                {
                    PlayerInventory.Instance.PickUpItem(hit.gameObject);
                }
            }
            
        }
    }
}