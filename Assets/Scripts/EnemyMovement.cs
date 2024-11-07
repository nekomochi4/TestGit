using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private bool isMovingRight = true; //false������獶�ɍs��

    private void Update()
    {
        // ���E�ړ�
        float direction = isMovingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �ǂɓ��������������ς���
        isMovingRight = !isMovingRight;
        FlipSprite();
    }

    private void FlipSprite()
    {
        // �L�����N�^�[�̌����𔽓]
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}