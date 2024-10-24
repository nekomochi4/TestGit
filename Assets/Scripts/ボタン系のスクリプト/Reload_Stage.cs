using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // ���̃X�e�[�W����ێ�����ϐ�
    private static string lastStage;
   
    // �Q�[�����I�������Ƃ��ɁA�X�e�[�W�����L�^����
    public static void SetLastStage(string stageName)
    {
        lastStage = stageName;
    }

    // "��蒼��" �{�^���������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void RestartLevel()
    {
        // �L�^���ꂽ�X�e�[�W��������΂��̃V�[�������[�h
        if (!string.IsNullOrEmpty(lastStage))
        {
            SceneManager.LoadScene(lastStage);
        }
        else
        {
            // ������X�e�[�W�����L�^����Ă��Ȃ������ꍇ�͌��݂̃V�[�����ă��[�h
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
