using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    private static string lastStage;

    // ステージ名を記録するメソッド
    public static void SetLastStage(string stageName)
    {
        lastStage = stageName;
    }

    // "やり直し" ボタンが押されたときに呼ばれるメソッド
    public void RestartLevel()
    {
        // 記録されたステージ名があればそのシーンをロード
        if (!string.IsNullOrEmpty(lastStage))
        {
            SceneManager.LoadScene(lastStage);
        }
        else
        {
            // ステージ名が記録されていない場合、現在のシーンを再ロード
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
