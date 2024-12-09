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
    public float bounceForce = 10f; // 踏みつけ後のジャンプ力
    public float playerHp = 150;//プレイヤーの体力（仮）
    private Rigidbody2D rb;
    public LayerMask GroundLayer;
    private bool isDead = false;
    private BoxCollider2D bxCol;//ここから11/28

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SaveCurrentStage();
        //PrintObjectHierarchy(gameObject);
    }

    void Update()
    {
        // アニメーション状態の更新
        UpdateAnimationState();


    }



    private void UpdateAnimationState()
    {
        // Jump (プレイヤーが地面にいる場合のみジャンプ可能)
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }

        // Run
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // 微小な速度は無視
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
            GetComponent<Animator>().SetInteger("state", 2); // ジャンプ中
        }
        else if (rb.velocity.y < -0.1f)
        {
            GetComponent<Animator>().SetInteger("state", 3); // 落下中
        }

        // Player Movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, rb.velocity.y);

        // Sprite Flip
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // 微小な速度は無視
        {
            GetComponent<SpriteRenderer>().flipX = rb.velocity.x < 0;
        }
    }

    // 地面に接しているかどうかを判定
    private bool isGrounded()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(c.bounds.center, c.bounds.size, 0f, Vector2.down, 0.1f, GroundLayer);
        return hit.collider != null;
    }
    //----------------------------------消すならここから---------------------------------------------
    //プレイヤーの死亡判定

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        //クリア時の処理
        if (obj.CompareTag("Frag"))//Fragタグのオブジェクトに触れたら
        {
            SceneManager.LoadScene("Claer_Scene");
        }

        //落下死
        if (obj.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene("Result_Scene");
        }

        //敵との当たり判定の処理　：未
        if (obj.CompareTag("Enemy"))
        {
            HitEnemy(obj);
        }

        if (obj.CompareTag("Patapata"))
        {
            HITPata(obj);
        }
    }

    //敵に当たった際と、踏みつけた時の処理
    private void HitEnemy(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        // プレイヤーの底辺が敵の上辺より上にある場合
        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("敵を踏みつけました: " + enemy.name);
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // 敵を落下させる
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.bodyType = RigidbodyType2D.Dynamic; // 重力を有効化
                enemyRb.gravityScale = 1.5f; // 必要なら重力倍率を調整
            }

            // プレイヤーを跳ねさせる
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc(); // プレイヤーがダメージを受ける
            Debug.Log("敵に接触してダメージを受けました");
        }
    }

    //プレイヤーの体力の計算
    private void PlayerHpCalc()
    {
        playerHp = playerHp - 30;
                Debug.Log("体力は残り" + playerHp);
        if (playerHp  <= 0)
        {
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void HITPata(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;

        // プレイヤーの底辺が敵の上辺より上にある場合
        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("敵を踏みつけました: " + enemy.name);

            // パタパタの落下処理を呼び出す
            PatapataMovement patapataMovement = enemy.GetComponent<PatapataMovement>();
            if (patapataMovement != null)
            {
                patapataMovement.StompedDown(gameObject); //ここの修正から
            }

            // プレイヤーを跳ねさせる
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc(); // プレイヤーがダメージを受ける
            Debug.Log("敵に接触してダメージを受けました");
        }
    }

    //---------------------------------------------ここまで----------------------------------------------
    public void SaveCurrentStage() {
            string currentStage = SceneManager.GetActiveScene().name;
            RestartGame.SetLastStage(currentStage);
            NextStage.SetNextStage(currentStage);
         }
}



