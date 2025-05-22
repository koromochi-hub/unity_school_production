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

    // “o˜^‚³‚ê‚é‚½‚Ñ‚ÉQueue‚É’Ç‰Á
    public void Register(SwitchBomb bomb)
    {
        switchBombs.Enqueue(bomb);
    }

    // ŒÄ‚Î‚ê‚½‚Æ‚«‚Éæ“ª‚Ì”š’e‚ð”š”­‚³‚¹‚é
    public void TriggerNext()
    {
        if (switchBombs.Count > 0)
        {
            SwitchBomb bomb = switchBombs.Dequeue();
            bomb.Trigger();
        }
        else
        {
            Debug.Log("‹N“®‚Å‚«‚éƒXƒCƒbƒ`”š’e‚Í‚ ‚è‚Ü‚¹‚ñ");
        }
    }
}
