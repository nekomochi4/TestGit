using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private bool isMovingRight = true;

    private void Update()
    {
        // ���E�ړ�
        float direction = isMovingRight ? 1 : -1;//�ǂ����ɓ�����direction�ϐ��Ō��肷��
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);//deltaTime�Ƃ���������̃X�s�[�h�œ���
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