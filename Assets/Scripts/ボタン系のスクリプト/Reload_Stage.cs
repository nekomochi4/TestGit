using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private static string lastStage;

    // �X�e�[�W�����L�^���郁�\�b�h
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
            // �X�e�[�W�����L�^����Ă��Ȃ��ꍇ�A���݂̃V�[�����ă��[�h
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
