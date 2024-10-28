using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // Player.cs����󂯎�鎟�̃X�e�[�W����ۑ�����ϐ�
    private static string nextStage;
    /*���̃R�[�h�̏C�����炪�����B�X�e�[�W�������܂��ۑ�����Ă��Ȃ�*/
    // Player.cs�Ŏ��̃X�e�[�W�����Z�b�g���郁�\�b�h
    public static void SetNextStage(string stageName) 
    {
        nextStage = stageName;
    }

    // "Next" �{�^�������������ɌĂ΂�郁�\�b�h
    public void LoadNextStage()
    {
        // ���̃X�e�[�W���Ɋ�Â��ď�������
        switch (nextStage)
        {
            case "Stage1":
                SceneManager.LoadScene("Stage2");
                break;
            case "Stage2":
                SceneManager.LoadScene("Stage3");
                break;
            case "Stage3":
                SceneManager.LoadScene("Stage4");
                break;
            case "Stage4":
                SceneManager.LoadScene("Stage5");
                break;
            case "Stage5":
                SceneManager.LoadScene("ALL_Clear_Scene");
                break;
            default:
                Debug.LogError("Unknown stage name: " + nextStage);
                break;
        }
    }
}
