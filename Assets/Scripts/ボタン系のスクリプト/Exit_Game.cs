using UnityEngine;

public class ExitGame : MonoBehaviour // �Q�[�����I�����邽�߂̃X�N���v�g
{
    public void Quit()
    {
#if UNITY_EDITOR
        // Unity�G�f�B�^�[��ł�Editor���~������
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �r���h��̃Q�[�����I��
        Application.Quit();
#endif

        Debug.Log("�Q�[���I�����Ăяo����܂���"); // �f�o�b�O�p
    }
}
