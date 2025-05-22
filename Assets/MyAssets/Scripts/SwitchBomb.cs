using UnityEngine;

public class SwitchBomb : TrapBase
{
    private void Start()
    {
        SwitchBombManager.Instance.Register(this);
    }

    public override void Trigger()
    {
        Debug.Log("スイッチ爆弾が起動！");
        // ここに爆発エフェクトや効果音、ダメージ処理など追加
        Destroy(gameObject); // 仮に爆発後に消す
    }
}