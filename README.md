# 夢売店 (DreamShop)

## ゲーム概要
- **プレイ環境**: PCブラウザ (WebGL) / Windows / macOS
- **ジャンル**: 2D タイムマネジメント / アクション
- **開発期間**: 7日間 (2026年3月)

## 開発環境
- **Game Engine**: Unity 6000.3.9f1
- **Language**: C#
- **Packages / Assets**:
  - DOTween 
  - TextMeshPro 

## クレジット
- **BGM&SE**: [DOVA-SYNDROME https://dova-s.jp/]


##スクリプト構成

### コアシステム
* **`GameStateManager.cs`**
  ゲームの進行状態（準備・通常営業・裏営業・終了）を統括するステートマシン。
* **`ChaosManager.cs`**
  裏営業への移行演出、アイテムの物理演算アクティブ化、および無重力空間のようなアイテムの飛散を制御。
* **`LevelSelectManager.cs`**
  ステージ選択画面のUI制御と、進行度に応じたシーン遷移を管理。

### プレイヤー制御 
* **`PlayerController.cs`**
  プレイヤーのWASDキーによる移動制御。
* **`PlayerInventory.cs`**
  プレイヤーが現在手に持っているアイテムのデータ保持と状態管理。
* **`MouseInteractor.cs`**
  マウスクリックによるRaycast判定を行い、アイテムの取得・棚への配置・ゴミ箱への廃棄・顧客への提出などの各種インタラクション。

### 顧客・注文システム
* **`CustomerBehavior.cs`**
  個々の顧客の移動、忍耐力の減少、ゲージUIの更新、怒りによる退店ロジックを制御。
* **`LevelQueueManager.cs`**
  顧客のスポーン間隔の制御、行列の進行処理、店内状況に応じたダイナミックな生成スピードの調整。
* **`OrderManager.cs`**
  顧客の注文生成、プレイヤーが提出したアイテムの正誤判定、スコア計算。

### アイテム
* **`ItemData.cs`**
  アイテムの種類を定義するデータクラス。
* **`ItemBox.cs`**
  段ボールから指定されたアイテムを無限に生成・排出するギミックを制御。
* **`ShelfSlot.cs`**
  陳列棚の各スロットにおけるアイテムの配置状況の保持と、取り出し時の判定。
* **`DreamItemBehavior.cs`**
  夢境において、宙を飛散するアイテムの特殊な挙動や画面外判定を管理。

### 演出
* **`AudioManager.cs`**
  ゲーム全体のBGMおよびSEの再生。
* **`CameraFollow.cs`**
  プレイヤーキャラクターに追従するカメラの移動処理。
* **`QuoteManager.cs`**
  ゲーム起動時に表示される名言。
* **`TitleSceneManager.cs`**
  タイトル画面のUI制御、およびゲームスタート時の処理。
* **`EasterEggManager.cs`**
  イースター・エッグの管理
