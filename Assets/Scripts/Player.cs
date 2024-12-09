using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    public float MoveSpeed = 3f;
    public float JumpForce = 15f;
    public float bounceForce = 10f; // ���݂���̃W�����v��
    public float playerHp = 150;//�v���C���[�̗̑́i���j
    private Rigidbody2D rb;
    public LayerMask GroundLayer;
    private bool isDead = false;
    private BoxCollider2D bxCol;//��������11/28

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SaveCurrentStage();
        //PrintObjectHierarchy(gameObject);
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
    //----------------------------------�����Ȃ炱������---------------------------------------------
    //�v���C���[�̎��S����

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        //�N���A���̏���
        if (obj.CompareTag("Frag"))//Frag�^�O�̃I�u�W�F�N�g�ɐG�ꂽ��
        {
            SceneManager.LoadScene("Claer_Scene");
        }

        //������
        if (obj.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene("Result_Scene");
        }

        //�G�Ƃ̓����蔻��̏����@�F��
        if (obj.CompareTag("Enemy"))
        {
            HitEnemy(obj);
        }

        if (obj.CompareTag("Patapata"))
        {
            HITPata(obj);
        }
    }

    //�G�ɓ��������ۂƁA���݂������̏���
    private void HitEnemy(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        // �v���C���[�̒�ӂ��G�̏�ӂ���ɂ���ꍇ
        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("�G�𓥂݂��܂���: " + enemy.name);
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // �G�𗎉�������
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.bodyType = RigidbodyType2D.Dynamic; // �d�͂�L����
                enemyRb.gravityScale = 1.5f; // �K�v�Ȃ�d�͔{���𒲐�
            }

            // �v���C���[�𒵂˂�����
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc(); // �v���C���[���_���[�W���󂯂�
            Debug.Log("�G�ɐڐG���ă_���[�W���󂯂܂���");
        }
    }

    //�v���C���[�̗̑͂̌v�Z
    private void PlayerHpCalc()
    {
        playerHp = playerHp - 30;
                Debug.Log("�̗͎͂c��" + playerHp);
        if (playerHp  <= 0)
        {
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void HITPata(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;

        // �v���C���[�̒�ӂ��G�̏�ӂ���ɂ���ꍇ
        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("�G�𓥂݂��܂���: " + enemy.name);

            // �p�^�p�^�̗����������Ăяo��
            PatapataMovement patapataMovement = enemy.GetComponent<PatapataMovement>();
            if (patapataMovement != null)
            {
                patapataMovement.StompedDown(gameObject); //�����̏C������
            }

            // �v���C���[�𒵂˂�����
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc(); // �v���C���[���_���[�W���󂯂�
            Debug.Log("�G�ɐڐG���ă_���[�W���󂯂܂���");
        }
    }

    public void SaveCurrentStage()
    {
        string currentStage = SceneManager.GetActiveScene().name;
        RestartGame.SetLastStage(currentStage);
        NextStage.SetNextStage(currentStage);
    }
    // �ΏۂƂȂ�^�O��
    public string targetTag = "GrowItem";

    // �Փˎ��̏���
    private void OnTriggerEnter(Collider other)
    {
        // �Փˑ���̃^�O���w��̂��̂��m�F
        if (other.CompareTag(targetTag))
        {
            // �v���C���[�̃T�C�Y��2�{�ɕύX
            transform.localScale *= 5.0f;

        }
    }

}



