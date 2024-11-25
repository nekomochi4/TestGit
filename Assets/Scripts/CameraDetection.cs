//���C���J�����ɉf���Ă邩�ǂ������肵�āA�f���Ă���EnemyMovement.cs�œ����o���B

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

        // ������Ԃł͈ړ��𖳌���
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }
    }

    private void Update()
    {
        if (IsVisibleToCamera())//���C���J�����ɉf���Ă���enemyMOvement��L����
        {
            if (enemyMovement != null && !enemyMovement.enabled)
            {
                enemyMovement.enabled = true;
                Debug.Log("�J�����ɉf���Ă܂�(enemyMovement.enabled = true)");
            }
        }
    }

    private bool IsVisibleToCamera()//���C���J�����ɉf���Ă���
    {
        if (!enemyRenderer.isVisible)
            return false; //�f���ĂȂ��ꍇ��false

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);//�X�N���[�����W�n�ɕϊ����āA�\���͈͓��Ȃ�true��Ԃ�
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}