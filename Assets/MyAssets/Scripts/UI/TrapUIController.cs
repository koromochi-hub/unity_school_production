// TrapUIController.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;


public class TrapUIController : MonoBehaviour
{
    [Header("UI �̎Q�� (�q�I�u�W�F�N�g�Ȃ�)")]
    [SerializeField] private Transform iconContainer;
    [SerializeField] TMP_Text countText;

    [Header("�g���b�v���̃A�C�R���ꗗ(TrapPrefabs �Ɠ���)")]
    [SerializeField] private GameObject[] trapIconPrefabs;

    // �Ď����� PlayerTrapController
    private PlayerTrapController playerTrap;
    private GameObject currentIconModel;

    /// <summary>
    /// Manager ����Ă�ŕR�Â���
    /// </summary>
    public void Initialize(PlayerTrapController ptc)
    {
        // �����ȑO�ɕR�Â��Ă��������
        if (playerTrap != null)
            playerTrap.OnTrapSwitched -= HandleTrapSwitched;

        playerTrap = ptc;
        // �C�x���g�o�^
        playerTrap.OnTrapSwitched += HandleTrapSwitched;
        // �����\��
        // �����I�ɍ�������x�n���h�����Ă�:
        HandleTrapSwitched(playerTrap.CurrentIndex,
                           playerTrap.GetCurrentCount(),
                           playerTrap.GetMaxCount());
    }

    void OnDestroy()
    {
        if (playerTrap != null)
            playerTrap.OnTrapSwitched -= HandleTrapSwitched;
    }

    private void HandleTrapSwitched(int index, int curCount, int maxCount)
    {
        // (1) �܂��ߋ��̃��f��������
        if (currentIconModel != null)
            Destroy(currentIconModel);

        // (2) �V�������f�����A�C�R���R���e�i�ɐ���
        if (index >= 0 && index < trapIconPrefabs.Length)
        {
            currentIconModel = Instantiate(
                trapIconPrefabs[index],
                iconContainer,
                worldPositionStays: false
            );

            // �@ �e�Ɠ������C���[�ɐ؂�ւ�
            SetLayerRecursively(currentIconModel.transform, iconContainer.gameObject.layer);

            // �A �\�[�g�O���[�v��ǉ����� UI �\�[�g���C���[���ŕ`��
            var sg = currentIconModel.AddComponent<SortingGroup>();
            sg.sortingLayerID = SortingLayer.NameToID("UI");
            sg.sortingOrder = 1;

            // �K�v�ɉ����ăX�P�[���^��]�������
            currentIconModel.transform.localPosition = Vector3.zero;
        }

        // �J�E���g�\��
        countText.text = $"{curCount} / {maxCount}";

        countText.color = curCount <= 0
            ? Color.red
            : Color.white;
    }

    /// <summary>
    /// transform �ȉ����ꊇ�� layer �𑵂���w���p�[
    /// </summary>
    void SetLayerRecursively(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        foreach (Transform c in t)
            SetLayerRecursively(c, layer);
    }



}
