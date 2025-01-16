using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("ƒV[ƒ“–¼‚ª‹ó‚Å‚·I");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}

