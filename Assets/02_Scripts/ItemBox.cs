using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject itemPrefab;
    
    [Header("Dream Shoot")]
    public float autoSpitInterval = 1.5f; // 每1.5秒
    private float spitTimer = 0f;

    void Update()
    {

        if (GameStateManager.Instance != null && GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Chaos)
        {
            spitTimer += Time.deltaTime;
            if (spitTimer >= autoSpitInterval)
            {
                spitTimer = 0f;
                SpitWildItem(); 
            }
        }
    }

  
    public GameObject TakeNewItem()
    {
        if (itemPrefab == null) return null;

   
        if (GameStateManager.Instance != null && GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Chaos)
        {
            return null;
        }

        GameObject newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        return newItem;
    }


    void SpitWildItem()
    {
        if (itemPrefab == null) return;

        GameObject wildItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        ItemData itemData = wildItem.GetComponent<ItemData>();
        DreamItemBehavior behavior = wildItem.GetComponent<DreamItemBehavior>();

        if (itemData != null && behavior != null)
        {
            behavior.enabled = true;
            
            Collider2D col = wildItem.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;

            Rigidbody2D rb = wildItem.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                
                rb.linearVelocity = new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 10f)); 
            }
        }
    }
}