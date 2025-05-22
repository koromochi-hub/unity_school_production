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

    // �o�^����邽�т�Queue�ɒǉ�
    public void Register(SwitchBomb bomb)
    {
        switchBombs.Enqueue(bomb);
    }

    // �Ă΂ꂽ�Ƃ��ɐ擪�̔��e�𔚔�������
    public void TriggerNext()
    {
        if (switchBombs.Count > 0)
        {
            SwitchBomb bomb = switchBombs.Dequeue();
            bomb.Trigger();
        }
        else
        {
            Debug.Log("�N���ł���X�C�b�`���e�͂���܂���");
        }
    }
}
