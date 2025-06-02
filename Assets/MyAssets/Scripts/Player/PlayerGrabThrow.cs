using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabThrow : MonoBehaviour
{
    [Header("掴む／投げる 設定")]
    [SerializeField, Tooltip("拾えるオブジェクトを検知する半径")]
    private float maxPickupDistance = 1.5f;

    [SerializeField, Tooltip("オブジェクトを持ったときアタッチするホールドポイント")]
    private Transform holdPoint;

    [SerializeField, Tooltip("押し続けたときの最大投擲力")]
    private float maxThrowForce = 15f;

    [SerializeField, Tooltip("投擲力をためる速さ (1秒で maxThrowForce まで上げるなら maxThrowForce / 1f)")]
    private float throwChargeSpeed = 15f;

    // --- 内部フィールド ---
    private GameObject heldObject = null;
    private Rigidbody heldRigidbody = null;
    private bool isCharging = false;
    private float currentThrowForce = 0f;
    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        if (holdPoint == null)
        {
            Debug.LogError("PlayerGrabThrow: HoldPoint が Inspector にセットされていません。");
        }
    }

    /// <summary>
    /// PlayerInput の「GrabThrow」アクションから呼び出されるメソッド
    /// </summary>
    public void OnGrabThrow(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (heldObject == null)
            {
                TryPickUp();
            }
            else
            {
                // すでに持っているなら投げるためチャージ開始
                isCharging = true;
                currentThrowForce = 0f;
            }
        }
        else if (context.canceled)
        {
            if (heldObject != null && isCharging)
            {
                ThrowHeldObject();
                isCharging = false;
                currentThrowForce = 0f;
            }
        }
    }

    private void Update()
    {
        // チャージ中でオブジェクトを持っているなら、力を溜める
        if (isCharging && heldObject != null)
        {
            currentThrowForce += throwChargeSpeed * Time.deltaTime;
            currentThrowForce = Mathf.Min(currentThrowForce, maxThrowForce);
        }

        // 持っているオブジェクトがあれば HoldPoint に追従
        if (heldObject != null && holdPoint != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    /// <summary>
    /// 近くの "Throwable" タグ付きオブジェクトを探して掴む
    /// </summary>
    private void TryPickUp()
    {
        Debug.Log("TryPickUp()が呼ばれました");
        Collider[] hits = Physics.OverlapSphere(transform.position, maxPickupDistance);
        float closestDist = Mathf.Infinity;
        GameObject nearest = null;

        foreach (var col in hits)
        {
            if (col.gameObject.CompareTag("Throwable"))
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearest = col.gameObject;
                }
            }
        }

        if (nearest != null)
        {
            PickUp(nearest);
        }
    }

    /// <summary>
    /// 実際にオブジェクトを掴む処理
    /// </summary>
    private void PickUp(GameObject obj)
    {
        heldObject = obj;
        heldRigidbody = obj.GetComponent<Rigidbody>();

        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = true;
            heldRigidbody.linearVelocity = Vector3.zero;
            heldRigidbody.angularVelocity = Vector3.zero;
        }

        // PickableObject 側のバウンスをリセット
        var pickable = obj.GetComponent<PickableObject>();
        if (pickable != null)
        {
            pickable.ResetBounce();
        }

        // HoldPoint にセットして親子関係を作る
        obj.transform.position = holdPoint.position;
        obj.transform.rotation = holdPoint.rotation;
        obj.transform.SetParent(holdPoint);
    }

    /// <summary>
    /// オブジェクトを投げる処理
    /// </summary>
    private void ThrowHeldObject()
    {
        if (heldObject == null) return;

        // 親子関係を解除
        heldObject.transform.SetParent(null);

        // Rigidbody を通常モードに戻す
        if (heldRigidbody != null)
        {
            heldRigidbody.isKinematic = false;
        }

        // 【投擲方向を transform.forward にする】
        Vector3 throwDir = transform.forward;

        if (heldRigidbody != null)
        {
            heldRigidbody.AddForce(throwDir * currentThrowForce, ForceMode.Impulse);
        }

        heldObject = null;
        heldRigidbody = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxPickupDistance);
    }
}
