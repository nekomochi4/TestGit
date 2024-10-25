using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // "Next" �{�^���������ꂽ�Ƃ��Ɏ��̃X�e�[�W�ɐi��
    public void LoadNextStage()
    {
        // ���݂̃V�[���̃r���h�C���f�b�N�X���擾���A���̃C���f�b�N�X�̃V�[�������[�h����
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}

