using System.Collections.Generic;
using UnityEngine;

public class SwitchBombManager : MonoBehaviour
{
    public static SwitchBombManager Instance;

    // プレイヤーごとのスイッチ爆弾を管理
    private Dictionary<PlayerStatus, List<SwitchBomb>> playerBombs = new Dictionary<PlayerStatus, List<SwitchBomb>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 爆弾の登録（設置者ごとに追加）
    public void Register(SwitchBomb bomb, PlayerStatus owner)
    {
        if (!playerBombs.ContainsKey(owner))
        {
            playerBombs[owner] = new List<SwitchBomb>();
        }
        playerBombs[owner].Add(bomb);
    }

    // 該当プレイヤーの全スイッチ爆弾を一斉に爆発
    public void TriggerAll(PlayerStatus owner)
    {
        if (!playerBombs.ContainsKey(owner)) return;

        var bombsToTrigger = new List<SwitchBomb>(playerBombs[owner]);

        foreach (var bomb in bombsToTrigger)
        {
            if (bomb != null) bomb.Trigger();
        }

        // 一度に全爆発させたので削除
        playerBombs[owner].Clear();
    }

    // 爆発後の手動削除
    public void Unregister(SwitchBomb bomb, PlayerStatus owner)
    {
        if (playerBombs.ContainsKey(owner))
        {
            playerBombs[owner].Remove(bomb);
        }
    }
}