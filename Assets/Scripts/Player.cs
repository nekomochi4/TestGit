using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float JumpForce = 15f;
    private Rigidbody2D rb;
    public LayerMask GroundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �A�j���[�V������Ԃ̍X�V
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // Jump (�v���C���[���n�ʂɂ���ꍇ�̂݃W�����v�\)
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }

        // Run
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // �����ȑ��x�͖���
        {
            GetComponent<Animator>().SetInteger("state", 1);
        }
        else
        {
            GetComponent<Animator>().SetInteger("state", 0);
        }

        // Jump / Fall
        if (rb.velocity.y > 0.1f)
        {
            GetComponent<Animator>().SetInteger("state", 2); // �W�����v��
        }
        else if (rb.velocity.y < -0.1f)
        {
            GetComponent<Animator>().SetInteger("state", 3); // ������
        }

        // Player Movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, rb.velocity.y);

        // Sprite Flip
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // �����ȑ��x�͖���
        {
            GetComponent<SpriteRenderer>().flipX = rb.velocity.x < 0;
        }
    }

    // �n�ʂɐڂ��Ă��邩�ǂ����𔻒�
    private bool isGrounded()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(c.bounds.center, c.bounds.size, 0f, Vector2.down, 0.1f, GroundLayer);
        return hit.collider != null;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            // ���݂̃X�e�[�W����ۑ�
            RestartGame.SetLastStage(SceneManager.GetActiveScene().name);

            // ���U���g�V�[���Ɉړ�����
            SceneManager.LoadScene("Result_Scene");
        }
    }

}
