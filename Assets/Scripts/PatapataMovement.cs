using UnityEngine;

public class PatapataMovement : MonoBehaviour
{
    public float moveSpeed = 2f;  // �ړ����x
    public float moveHeight = 3f; // �ړ��̍���

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 startPosition;
    private bool canMove = true;

    void Start()
    {
        // Rigidbody2D�R���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>();

        // Collider2D�R���|�[�l���g���擾
        col = GetComponent<Collider2D>();

        // �����ʒu��ۑ�
        startPosition = rb.position;

        // �d�͂̉e���𖳌��ɂ���
        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {
        // canMove��true�̏ꍇ�̂ݏ㉺�Ɉړ�
        if (canMove)
        {
            // Sin�֐����g���ď㉺�̈ʒu���v�Z
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;

            // �V�����ʒu��ݒ�iX, Z�͕ύX���Ȃ��j
            rb.MovePosition(new Vector2(startPosition.x, newY));
        }
    }

    // �v���C���[�ɓ��݂���ꂽ���̏���
    public void StompedDown(GameObject enemy)
    {
        canMove = false; // �ړ����~
        rb.bodyType = RigidbodyType2D.Dynamic; // �d�͂�L����
        rb.gravityScale = 1.5f; // ��������悤�ɏd�͂�ݒ�

        // �����蔻��𖳌���
        if (col != null)
        {
            col.enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        // �����蔻��̃M�Y����\��
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
