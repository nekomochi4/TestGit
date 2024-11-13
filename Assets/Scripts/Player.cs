using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //UI���g���Ƃ��ɏ����܂��B
using UnityEngine.UI; //UI���g���Ƃ��ɏ����܂��B

public class Player : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float JumpForce = 15f;
    private Rigidbody2D rb;
    public LayerMask GroundLayer;

   

   

    void Start()
    {
   
      

   
      

        rb = GetComponent<Rigidbody2D>();
        SaveCurrentStage();
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
            // ���S�����烊�U���g�V�[���Ɉړ�����
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        // �t���O�A�C�e���ɐڐG�����ꍇ�A�N���A�V�[���Ɉړ�����
        if (obj.CompareTag("Frag"))
        {
            SceneManager.LoadScene("Clear_Scene");
        }
    }


    public void SaveCurrentStage()
    {
        string currentStage = SceneManager.GetActiveScene().name;
        RestartGame.SetLastStage(currentStage);
        NextStage.SetNextStage(currentStage);
    }
}
