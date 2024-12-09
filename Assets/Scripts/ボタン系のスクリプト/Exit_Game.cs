using UnityEngine;

public class ExitGame : MonoBehaviour // ゲームを終了するためのスクリプト
{
    public void Quit()
    {
#if UNITY_EDITOR
        // Unityエディター上ではEditorを停止させる
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後のゲームを終了
        Application.Quit();
#endif

        Debug.Log("ゲーム終了が呼び出されました"); // デバッグ用
    }
}
