// PlayerStatus.cs
using UnityEngine;
using System;

public class PlayerStatus : MonoBehaviour
{
    public int playerId;

    [SerializeField] int maxHP = 100;
    public int MaxHP => maxHP;

    int currentHP;
    public int CurrentHP
    {
        get => currentHP;
        set
        {
            int v = Mathf.Clamp(value, 0, MaxHP);
            if (v == currentHP) return;
            currentHP = v;
            OnHPChanged?.Invoke(currentHP, MaxHP);
        }
    }

    // HP変化を通知するイベント
    public event Action<int, int> OnHPChanged;

    void Awake()
    {
        // 初期化
        currentHP = maxHP;
    }

    // ヘルパー
    public void TakeDamage(int d) => CurrentHP -= d;
    public void Heal(int h) => CurrentHP += h;
}
