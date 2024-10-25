using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // "Next" ボタンが押されたときに次のステージに進む
    public void LoadNextStage()
    {
        // 現在のシーンのビルドインデックスを取得し、次のインデックスのシーンをロードする
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}

