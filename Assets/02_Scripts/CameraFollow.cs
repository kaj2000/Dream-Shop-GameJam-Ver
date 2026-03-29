using UnityEngine;

/// <summary>
/// カメラがプレイヤーを滑らかに追従するクラス
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("ターゲット")]
    public Transform target;

    [Header("滑らかさ")]
    public float smoothSpeed = 0.125f;

    [Header("カメラの位置ズレ")]
    public Vector3 offset = new Vector3(0, 0, -10f);

    void FixedUpdate()
    {
        if (target != null)
        {
            // ターゲットの現在地 + オフセット
            Vector3 desiredPosition = target.position + offset;
            // 現在地から目標位置まで滑らかに移動
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
    }
}