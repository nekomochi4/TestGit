using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody2D rb;
    public LayerMask GroundLayer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SaveCurrentStage();
        PrintObjectHierarchy(gameObject);
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

    // �f�o�b�O�p�F�I�u�W�F�N�g�̊K�w�\����\��
    private void PrintObjectHierarchy(GameObject obj, string indent = "")
    {
        string info = $"{indent}Object: {obj.name}, Tag: {obj.tag}, Layer: {LayerMask.LayerToName(obj.layer)}";
        Debug.Log(info);

        // �e�I�u�W�F�N�g�̏��
        if (obj.transform.parent != null)
        {
            Debug.Log($"{indent}Parent: {obj.transform.parent.name}");
        }

        // �R���|�[�l���g���
        Component[] components = obj.GetComponents<Component>();
        foreach (Component comp in components)
        {
            Debug.Log($"{indent}Component: {comp.GetType().Name}");
        }

        // �q�I�u�W�F�N�g���ċA�I�ɕ\��
        foreach (Transform child in obj.transform)
        {
            PrintObjectHierarchy(child.gameObject, indent + "  ");
        }


    } //�����܂łŃo�b�N�p

    private void OnCollisionEnter2D_(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            // ���S�����烊�U���g�V�[���Ɉړ�����
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;  // ���Ɏ��S�������Ȃ疳��

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // �Փ˂����ʒu���m�F
            Vector2 hitPoint = collision.contacts[0].point;
            Vector2 playerBottom = new Vector2(transform.position.x, GetComponent<Collider2D>().bounds.min.y);

            // �v���C���[�̑������班����܂ł𓥂݂�����Ƃ���
            float stompThreshold = 0.1f; // �K�X�������Ă�������

            if (rb.velocity.y < 0 && hitPoint.y < (transform.position.y - stompThreshold))
            {
                // ���݂�����
                HandleEnemyStomped(collision.gameObject);
            }
            else
            {
                // �G�ɓ������ă_���[�W
                HandlePlayerDeath();
            }
        }
        else if (collision.gameObject.CompareTag("InstaDeath"))
        {
            HandlePlayerDeath();
        }
    }

    private void HandleEnemyStomped(GameObject enemy)
    {
        // �G�𖳌���
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
        if (enemySprite != null)
        {
            enemySprite.enabled = false;
        }

        // �R���C�_�[�𖳌���
        Collider2D[] colliders = enemy.GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        // ���˕Ԃ�
        rb.velocity = new Vector2(rb.velocity.x, bounceForce);

        // �x�����ēG�����S�ɍ폜
        StartCoroutine(DelayedDestroyEnemy(enemy));
    }

    private void HandlePlayerDeath()
    {
        if (isDead) return;

        isDead = true;
        // ���S�A�j���[�V�����Ȃǂ�ǉ��ł��܂�
        StartCoroutine(DelayedLoadResult());
    }

    private IEnumerator DelayedLoadResult()
    {
        // �����҂��Ă���V�[���J�ځi�K�v�ɉ����Ē����j
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Result_Scene");
    }

    private IEnumerator DelayedDestroyEnemy(GameObject enemy)
    {
        Debug.Log($"Destroy target: {enemy.name}");
        yield return new WaitForSeconds(0.5f);

        if (enemy != null && enemy.CompareTag("Enemy")) // �^�O�����������Ċm�F
        {
            Destroy(enemy);
        }
        else
        {
            Debug.LogWarning("Destroy skipped: Object is null or not tagged as 'Enemy'");
        }
    }


    // �V�[���J�n���̏�����
    private void OnEnable()
    {
        isDead = false;
    }

public void SaveCurrentStage()
    {
        string currentStage = SceneManager.GetActiveScene().name;
        RestartGame.SetLastStage(currentStage);
        NextStage.SetNextStage(currentStage);
    }
}



