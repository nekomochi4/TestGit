using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // 次のステージの名前をInspectorで設定できるようにする
    public string nextStageName;

    // "Next" ボタンが押されたときに次のステージに進む
    public void LoadNextStage()
    {
        // 指定された次のステージをロードする
        SceneManager.LoadScene(nextStageName);
    }
}
