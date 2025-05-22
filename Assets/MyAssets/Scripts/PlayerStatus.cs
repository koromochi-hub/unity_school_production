using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private int MaxHP = 100;
    public int CurrentHP { get; private set; }

    private void Awake()
    {
        CurrentHP = MaxHP;
    }

    public void TakeDamage(int damage, Vector3 knockbackDirection, float knockbackForce)
    {
        CurrentHP -= damage;
        if (CurrentHP < 0) CurrentHP = 0;

        rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        Debug.Log($"{gameObject.name} はダメージを受けた！ 残りHP: {CurrentHP}");

        // 死亡処理などが必要ならここに追加
    }
}
