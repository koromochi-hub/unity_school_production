using System.Collections.Generic;
using UnityEngine;

public class SwitchBombManager : MonoBehaviour
{
    public static SwitchBombManager Instance;

    private Queue<SwitchBomb> switchBombs = new Queue<SwitchBomb>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 登録されるたびにQueueに追加
    public void Register(SwitchBomb bomb)
    {
        switchBombs.Enqueue(bomb);
    }

    // 呼ばれたときに先頭の爆弾を爆発させる
    public void TriggerNext()
    {
        if (switchBombs.Count > 0)
        {
            SwitchBomb bomb = switchBombs.Dequeue();
            bomb.Trigger();
        }
        else
        {
            Debug.Log("起動できるスイッチ爆弾はありません");
        }
    }
}
