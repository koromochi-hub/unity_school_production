using UnityEngine;

public class SwitchBomb : TrapBase
{
    private void Start()
    {
        SwitchBombManager.Instance.Register(this);
    }

    public override void Trigger()
    {
        Debug.Log("�X�C�b�`���e���N���I");
        // �����ɔ����G�t�F�N�g����ʉ��A�_���[�W�����Ȃǒǉ�
        Destroy(gameObject); // ���ɔ�����ɏ���
    }
}