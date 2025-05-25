using System.Collections.Generic;
using UnityEngine;

public class SwitchBombManager : MonoBehaviour
{
    public static SwitchBombManager Instance;

    // �v���C���[���Ƃ̃X�C�b�`���e���Ǘ�
    private Dictionary<PlayerStatus, List<SwitchBomb>> playerBombs = new Dictionary<PlayerStatus, List<SwitchBomb>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ���e�̓o�^�i�ݒu�҂��Ƃɒǉ��j
    public void Register(SwitchBomb bomb, PlayerStatus owner)
    {
        if (!playerBombs.ContainsKey(owner))
        {
            playerBombs[owner] = new List<SwitchBomb>();
        }
        playerBombs[owner].Add(bomb);
    }

    // �Y���v���C���[�̑S�X�C�b�`���e����Ăɔ���
    public void TriggerAll(PlayerStatus owner)
    {
        if (!playerBombs.ContainsKey(owner)) return;

        var bombsToTrigger = new List<SwitchBomb>(playerBombs[owner]);

        foreach (var bomb in bombsToTrigger)
        {
            if (bomb != null) bomb.Trigger();
        }

        // ��x�ɑS�����������̂ō폜
        playerBombs[owner].Clear();
    }

    // ������̎蓮�폜
    public void Unregister(SwitchBomb bomb, PlayerStatus owner)
    {
        if (playerBombs.ContainsKey(owner))
        {
            playerBombs[owner].Remove(bomb);
        }
    }
}