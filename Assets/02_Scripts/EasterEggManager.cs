using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using DG.Tweening; 

/// <summary>
/// 小绵羊彩蛋 Sheep Easter Egg Manager
/// </summary>
public class EasterEggUIManager : MonoBehaviour
{
    [Header("Sheep Prefab")]
    public GameObject uiSheepPrefab;

    [Header("rythm")]
    public float minSpawnInterval = 5f; 
    public float maxSpawnInterval = 15f; 
    
    [Header("run speed")]
    public float runSpeed = 200f; // run speed

    [Header("Canvas")]
    private Vector2 referenceResolution = new Vector2(1920, 1080);

    private Coroutine spawnCoroutine;
    private RectTransform myRectTransform;

    void Awake()
    {
        myRectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {

        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnUISheepLoop());
        Debug.Log("绵羊开始奔跑...");
    }

    void OnDisable()
    {

        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    /// <summary>
    /// ひつじ循環
    /// </summary>
    IEnumerator SpawnUISheepLoop()
    {
        while (true)
        {
            // 1. 等待随机时间
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            // 2. 只有在梦境时刻才生成
            if (GameStateManager.Instance != null && GameStateManager.Instance.currentPhase == GameStateManager.GamePhase.Chaos)
            {
                SpawnSingleUISheep();
            }
        }
    }

    void SpawnSingleUISheep()
    {
        if (uiSheepPrefab == null) return;

        // 3. 随机逻辑
        bool moveRightToLeft = (Random.Range(0, 100) > 50);

        // 4. 生成绵羊并设为 Canvas 的子物体
        GameObject sheep = Instantiate(uiSheepPrefab, this.transform);
        RectTransform sheepRect = sheep.GetComponent<RectTransform>();

        // 5. 决定跑上面还是跑下面
        bool isTopRun = (Random.Range(0, 100) > 50);
        float verticalYOffset = -50f; 

        if (isTopRun)
        {
            // 锚点设为 Top Center
            sheepRect.anchorMin = new Vector2(0.5f, 1f);
            sheepRect.anchorMax = new Vector2(0.5f, 1f);
            sheepRect.pivot = new Vector2(0.5f, 1f); // 轴心也在上面
            verticalYOffset = -Mathf.Abs(verticalYOffset); // Top往下移
        }
        else
        {
          
            sheepRect.anchorMin = new Vector2(0.5f, 0f);
            sheepRect.anchorMax = new Vector2(0.5f, 0f);
            sheepRect.pivot = new Vector2(0.5f, 0f); // 轴心也在下面
            verticalYOffset = Mathf.Abs(verticalYOffset); // Bottom往上移
        }

        // 6. 确定在 Canvas 参考尺寸下的起点和终点
        float halfWidth = referenceResolution.x / 2f + sheepRect.sizeDelta.x; 
        float startX = moveRightToLeft ? halfWidth : -halfWidth;
        float endX = moveRightToLeft ? -halfWidth : halfWidth;

        // 设置初始位置
        sheepRect.anchoredPosition = new Vector2(startX, verticalYOffset);

        // 7. 处理贴图翻转
        if (moveRightToLeft)
        {
            Vector3 scale = sheepRect.localScale;
            scale.x = -Mathf.Abs(scale.x); // 保证 X 轴缩放为负
            sheepRect.localScale = scale;
        }

        // 8. DOTween 演出 
        float distance = Mathf.Abs(endX - startX);
        float duration = distance / runSpeed;

        // 让绵羊直线匀速
        sheepRect.DOAnchorPosX(endX, duration)
            .SetEase(Ease.Linear) // 匀速
            .OnComplete(() => 
            {
                // 9. 销毁
                Destroy(sheep);
            });
    }
}