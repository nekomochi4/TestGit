using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float JumpForce = 15f;
    public float bounceForce = 10f; // 踏みつけたときのジャンプ力
    public float playerHp = 150; // プレイヤーの体力（初期値）
    private Rigidbody2D rb;
    public LayerMask GroundLayer;
    private bool isDead = false;
    private string stageName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SaveCurrentStage();
    }

    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // ジャンプ
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }

        // アニメーション状態の更新
        Animator animator = GetComponent<Animator>();
        if (Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            animator.SetInteger("state", 1); // 移動中
        }
        else
        {
            animator.SetInteger("state", 0); // 静止中
        }

        if (rb.velocity.y > 0.1f)
        {
            animator.SetInteger("state", 2); // ジャンプ中
        }
        else if (rb.velocity.y < -0.1f)
        {
            animator.SetInteger("state", 3); // 落下中
        }

        // プレイヤーの移動処理
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, rb.velocity.y);

        // スプライトの左右反転処理
        if (Mathf.Abs(rb.velocity.x) > 0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = rb.velocity.x < 0;
        }
    }

    private bool isGrounded()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(c.bounds.center, c.bounds.size, 0f, Vector2.down, 0.1f, GroundLayer);
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Frag"))
        {
            Debug.Log("旗と接触しました");
            if (stageName == "Stage5")
            {
                SceneManager.LoadScene("All_Clear_Scene");
            }
            else
            {
                SceneManager.LoadScene("Clear_Scene");
            }
        }

        if (obj.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene("Result_Scene");
        }

        if (obj.CompareTag("Enemy"))
        {
            HitEnemy(obj);
        }

        if (obj.CompareTag("Patapata"))
        {
            HITPata(obj);
        }
    }

    private void HitEnemy(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;

        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("敵を踏みつけました: " + enemy.name);

            Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.bodyType = RigidbodyType2D.Dynamic;
                enemyRb.gravityScale = 1.5f;
            }

            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc();
            Debug.Log("敵に接触してダメージを受けました");
        }
    }

    private void HITPata(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;

        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("パタパタを踏みつけました: " + enemy.name);

            PatapataMovement patapataMovement = enemy.GetComponent<PatapataMovement>();
            if (patapataMovement != null)
            {
                patapataMovement.StompedDown(gameObject);
            }

            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc();
            Debug.Log("パタパタに接触してダメージを受けました");
        }
    }

    private void PlayerHpCalc()
    {
        playerHp -= 30;
        Debug.Log("現在の体力: " + playerHp);
        if (playerHp <= 0)
        {
            SceneManager.LoadScene("Result_Scene");
        }
    }

    public void SaveCurrentStage()
    {
        string currentStage = SceneManager.GetActiveScene().name;
        RestartGame.SetLastStage(currentStage);
        NextStage.SetNextStage(currentStage);
        stageName = currentStage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GrowItem"))
        {
            transform.localScale *= 5.0f;
        }
    }
}
