using UnityEngine;

/// <summary>
/// アイテムの種類
/// </summary>
public class ItemData : MonoBehaviour
{
    // アイテムの基本種類
    public enum ItemType
    {
        Onigiri,   // おにぎり
        Cola,       // コーラ
        CupNoodle    // 杯面
    }

    // 夢の中の異常状態
    public enum DreamModifier
    {
        Normal,     // 普通
        Floating,   // 浮遊している
        Fleeing     // 逃げ回る
    }

    [Header("Item set")]
    public ItemType currentType;
    public DreamModifier currentModifier;


    [HideInInspector] 
    public bool isCaught = false; 


}