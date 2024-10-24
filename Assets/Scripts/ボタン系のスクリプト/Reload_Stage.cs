using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // 元のステージ名を保持する変数
    private static string lastStage;
   
    // ゲームが終了したときに、ステージ名を記録する
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
            // 万が一ステージ名が記録されていなかった場合は現在のシーンを再ロード
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
