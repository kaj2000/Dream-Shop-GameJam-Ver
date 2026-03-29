using UnityEngine;

/// <summary>
/// プレイヤーの移動と入力を管理するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("move speed")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    
    private Animator animator; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        if (movement != Vector2.zero)
        {
            // 動いている時
            animator.SetFloat("MoveX", movement.x);
            animator.SetFloat("MoveY", movement.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            // 止まっている時
            animator.SetBool("isMoving", false);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}