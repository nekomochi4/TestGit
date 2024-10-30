using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    private static string nextStage;

    // 現在のステージに基づき、次のステージ名をセットするメソッド
    public static void SetNextStage(string currentStage)
    {
        switch (currentStage)
        {
            case "Stage1":
                nextStage = "Stage2";
                break;
            case "Stage2":
                nextStage = "Stage3";
                break;
            case "Stage3":
                nextStage = "Stage4";
                break;
            case "Stage4":
                nextStage = "Stage5";
                break;
            case "Stage5":
                nextStage = "ALL_Clear_Scene";
                break;
            default:
                Debug.LogError("Unknown stage name: " + currentStage);
                break;
        }
    }

    // ボタンを押した時に呼ばれるメソッドで、次のステージに遷移
    public void LoadNextStage()
    {
        if (!string.IsNullOrEmpty(nextStage))
        {
            SceneManager.LoadScene(nextStage);
        }
        else
        {
            Debug.LogError("Next stage not set!");
        }
    }
}
