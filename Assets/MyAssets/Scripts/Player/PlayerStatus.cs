using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("HP設定")]
    [Tooltip("最大HP")]
    [SerializeField] private int maxHP = 100;

    [SerializeField] private PlayerMove playerMove;

    public int playerId;
    private int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

  
    public void TakeDamage(int damage)
    {
        // HP を減少
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        Debug.Log($"{gameObject.name} は {damage} ダメージを受けた。残りHP: {currentHP}");

    }
}

