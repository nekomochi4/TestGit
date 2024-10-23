using UnityEngine;

public class BackgroundFollowCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 offset;

    void Start()
    {
        // �J������Transform���擾
        cameraTransform = Camera.main.transform;

        // �w�i�ƃJ�����̏����ʒu�̍���ۑ�
        offset = transform.position - cameraTransform.position;
    }

    void LateUpdate()
    {
        // �w�i�̈ʒu���J�����̈ʒu�ɍ��킹�čX�V!
        transform.position = new Vector3(cameraTransform.position.x + offset.x, cameraTransform.position.y + offset.y, transform.position.z);
    }
}
