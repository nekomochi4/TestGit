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

        // ‰Šúó‘Ô‚Å‚ÍˆÚ“®‚ð–³Œø‰»
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }
    }

    private void Update()
    {
        if (IsVisibleToCamera())
        {
            if (enemyMovement != null && !enemyMovement.enabled)
            {
                enemyMovement.enabled = true;
                Debug.Log("Enemy detected by camera - starting movement");
            }
        }
    }

    private bool IsVisibleToCamera()
    {
        if (!enemyRenderer.isVisible)
            return false;

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}