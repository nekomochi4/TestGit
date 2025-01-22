using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bgm : MonoBehaviour
{
    string stageName; //
    // Use this for initialization
    void Start()
    {
        getStageName();
        if (stageName != "Stage5")
        {
            //画面遷移してもオブジェクトが壊れないようにする
            DontDestroyOnLoad(this);
        }else
        {
            Destroy(this);
        }
    }

    public void getStageName()
    {
        stageName = SceneManager.GetActiveScene().name;
    }



}