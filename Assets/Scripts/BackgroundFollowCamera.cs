using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 offset;

    void Start()
    {
        // カメラのTransformを取得
        cameraTransform = Camera.main.transform;

        // 背景とカメラの初期位置の差を保存
        offset = transform.position - cameraTransform.position;
    }

    void LateUpdate()
    {
        // 背景の位置をカメラの位置に合わせて更新!
        transform.position = new Vector3(cameraTransform.position.x + offset.x, cameraTransform.position.y + offset.y, transform.position.z);
    }
}
