using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // ���̃X�e�[�W�̖��O��Inspector�Őݒ�ł���悤�ɂ���
    public string nextStageName;

    // "Next" �{�^���������ꂽ�Ƃ��Ɏ��̃X�e�[�W�ɐi��
    public void LoadNextStage()
    {
        // �w�肳�ꂽ���̃X�e�[�W�����[�h����
        SceneManager.LoadScene(nextStageName);
    }
}
