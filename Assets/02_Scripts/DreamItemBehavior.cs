using UnityEngine;

public class DreamItemBehavior : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    private ItemData itemData;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemData = GetComponent<ItemData>();
        
        SetRandomVelocity();
    }

    void Update()
    {
        if (itemData != null && itemData.isCaught)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            this.enabled = false;
            return;
        }

        if (rb != null)
        {
            if (rb.linearVelocity.magnitude < 0.5f)
            {
                SetRandomVelocity();
            }
            else
            {
               
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
    }

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        SetRandomVelocity();
    }

    void SetRandomVelocity()
    {
        if (rb != null)
        {
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            if (randomDir == Vector2.zero) randomDir = Vector2.up;
            rb.linearVelocity = randomDir * moveSpeed;
        }
    }
}