using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    // Player.csから受け取る次のステージ名を保存する変数
    private static string nextStage;
    /*このコードの修正からが続き。ステージ名がうまく保存されていない*/
    // Player.csで次のステージ名をセットするメソッド
    public static void SetNextStage(string stageName) 
    {
        nextStage = stageName;
    }

    // "Next" ボタンを押した時に呼ばれるメソッド
    public void LoadNextStage()
    {
        // 次のステージ名に基づいて条件分岐
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
