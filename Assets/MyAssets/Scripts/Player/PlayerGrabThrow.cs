using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerGrabThrow : MonoBehaviour
{
    [Header("掴む（Grab）設定")]
    [Tooltip("掴めるオブジェクトのタグ (Pickable など)")]
    [SerializeField] private string pickableTag = "Pickable";

    [Tooltip("掴む検出範囲 (メートル)")]
    [SerializeField] private float grabRange = 1.0f;

    [Tooltip("掴むときに使うレイヤーマスク (Pickable オブジェクトのレイヤー)")]
    [SerializeField] private LayerMask pickableLayerMask;

    [Tooltip("オブジェクトを持つ位置")]
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform carryPoint;

    [Header("投げる（Throw）設定")]
    [Tooltip("投げるときの最小初速")]
    [SerializeField] private float minThrowForce = 5f;

    [Tooltip("投げるときの最大初速")]
    [SerializeField] private float maxThrowForce = 15f;

    [Tooltip("ボタン長押しによる最大チャージ時間 (秒)")]
    [SerializeField] private float maxChargeTime = 1.5f;

    // ────── 内部キャッシュ ──────
    private PlayerInput playerInput;
    private InputAction grabAction;

    private GameObject carryingObject = null;   // 現在掴んでいるオブジェクト
    private Rigidbody carryRb = null;            // そのオブジェクトの Rigidbody

    private float grabChargeTimer = 0f;       // 「ボタン長押し」による投擲強度チャージ用タイマー
    private bool isChargingThrow = false;     // 現在投げる強度をチャージしているか

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            Debug.LogError("GrabThrow: PlayerInput がアタッチされていません。");

        grabAction = playerInput.actions["GrabThrow"];
        if (grabAction == null)
            Debug.LogError("GrabThrow: PlayerInput の ActionMap に 'GrabThrow' がありません。");

        if (carryPoint == null)
            Debug.LogError("GrabThrow: carryPoint がアサインされていません。");
    }

    private void OnEnable()
    {
        grabAction.started += OnGrabStarted;
        grabAction.canceled += OnGrabCanceled;
    }

    private void OnDisable()
    {
        grabAction.started -= OnGrabStarted;
        grabAction.canceled -= OnGrabCanceled;
    }

    private void Update()
    {
        // (1) 投擲チャージ処理：ボタンを押し続けている間だけタイマーを増やす
        if (isChargingThrow && carryingObject != null)
        {
            grabChargeTimer += Time.deltaTime;
            if (grabChargeTimer > maxChargeTime)
                grabChargeTimer = maxChargeTime;
        }
    }

    /// <summary>
    /// グラブ用ボタンを押した瞬間 (started)
    /// → まだ何も掴んでいなければ “掴み” をトライ
    /// → すでに掴んでいたら“投げ”のチャージを開始
    /// </summary>
    private void OnGrabStarted(InputAction.CallbackContext context)
    {
        if (carryingObject == null)
        {
            TryGrab();
        }
        else
        {
            // すでにオブジェクトを持っている → 投擲のチャージ開始
            isChargingThrow = true;
            grabChargeTimer = 0f;
        }
    }

    /// <summary>
    /// グラブ用ボタンを離した瞬間 (canceled)
    /// → もしオブジェクトを持っていれば「投げる」処理を実行
    /// </summary>
    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        if (carryingObject != null && isChargingThrow)
        {
            ThrowObject();
        }

        isChargingThrow = false;
    }

    /// <summary>
    /// 付近の Pickable オブジェクトを探して掴む
    /// </summary>
    private void TryGrab()
    {
        // 1) 手元位置から grabRange 半径の OverlapSphere を実行
        Collider[] hits = Physics.OverlapSphere(grabPoint.position, grabRange, pickableLayerMask);
        if (hits.Length == 0) return;

        // 2) 最初に見つかった Pickable タグ付きオブジェクトを掴む
        GameObject target = null;
        foreach (var col in hits)
        {
            if (col.CompareTag(pickableTag))
            {
                target = col.gameObject;
                break;
            }
        }
        if (target == null) return;

        // 3) 掴む処理：Rigidbody を isKinematic にして持ち上げる
        carryingObject = target;
        carryRb = carryingObject.GetComponent<Rigidbody>();
        if (carryRb != null)
        {
            carryRb.linearVelocity = Vector3.zero;
            carryRb.angularVelocity = Vector3.zero;
            carryRb.useGravity = false;
            carryRb.isKinematic = true;
        }

        // 4) プレイヤーの carryPoint の子にする＆位置リセット
        carryingObject.transform.SetParent(carryPoint);
        carryingObject.transform.localPosition = Vector3.zero;
        carryingObject.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 掴んでいるオブジェクトを「投げる」処理
    /// </summary>
    private void ThrowObject()
    {
        // 1) チャージ時間をもとに強度を計算
        float t = Mathf.Clamp01(grabChargeTimer / maxChargeTime);
        float force = Mathf.Lerp(minThrowForce, maxThrowForce, t);

        // 2) 持っているオブジェクトを carryPoint から外す
        carryingObject.transform.SetParent(null);

        if (carryRb != null)
        {
            // 3) 物理挙動を復活させる
            carryRb.isKinematic = false;
            carryRb.useGravity = true;

            // 4) 「投げられた」状態を PickableObject に伝える
            var pickable = carryingObject.GetComponent<PickableObject>();
            if (pickable != null)
            {
                pickable.SetThrown(true);
            }

            // 5) Forward ベクトル (プレイヤーの向き) に沿って飛ばす
            Vector3 throwDir = transform.forward;
            carryRb.AddForce(throwDir * force, ForceMode.Impulse);
        }

        // 6) 持っている状態をクリア
        carryingObject = null;
        carryRb = null;
        grabChargeTimer = 0f;
    }

    // もしギズモ表示したい場合は以下を有効化
    private void OnDrawGizmosSelected()
    {
        if (carryPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(grabPoint.position, grabRange);
        }
    }
}
