//メインカメラに映ってるかどうか判定して、映ってたらEnemyMovement.csで動き出す。

using UnityEngine;

public class CameraDetection : MonoBehaviour 
{
    private Camera mainCamera;
    private Renderer enemyRenderer;
    private EnemyMovement enemyMovement;
    
    private void Start()
    {
        mainCamera = Camera.main;
        enemyRenderer = GetComponent<Renderer>();
        enemyMovement = GetComponent<EnemyMovement>();

        // 初期状態では移動を無効化
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }
    }

    private void Update()
    {
        if (IsVisibleToCamera())//メインカメラに映ってたらenemyMOvementを有効化
        {
            if (enemyMovement != null && !enemyMovement.enabled)
            {
                enemyMovement.enabled = true;
                Debug.Log("カメラに映ってます(enemyMovement.enabled = true)");
            }
        }
    }

    private bool IsVisibleToCamera()//メインカメラに映ってたら
    {
        if (!enemyRenderer.isVisible)
            return false; //映ってない場合はfalse

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);//スクリーン座標系に変換して、表示範囲内ならtrueを返す
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}