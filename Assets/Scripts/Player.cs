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
    public float bounceForce = 10f; // 踏みつけたときのジャンプ力
    public float playerHp = 150; // プレイヤーの体力（初期値）
    private Rigidbody2D rb;
    public LayerMask GroundLayer;
    private bool isDead = false;
    private BoxCollider2D bxCol; // プレイヤーのBoxCollider2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SaveCurrentStage();
        // オブジェクト階層のデバッグ用出力
        //PrintObjectHierarchy(gameObject);
    }

    void Update()
    {
        // アニメーション状態の更新
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // ジャンプ（プレイヤーが地面にいる場合のみジャンプ可能）
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }

        // Runアニメーションの状態設定
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // 横移動速度がわずかでもある場合
        {
            GetComponent<Animator>().SetInteger("state", 1);
        }
        else
        {
            GetComponent<Animator>().SetInteger("state", 0);
        }

        // ジャンプまたは落下中のアニメーション状態設定
        if (rb.velocity.y > 0.1f)
        {
            GetComponent<Animator>().SetInteger("state", 2); // ジャンプ中
        }
        else if (rb.velocity.y < -0.1f)
        {
            GetComponent<Animator>().SetInteger("state", 3); // 落下中
        }

        // プレイヤーの移動処理
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, rb.velocity.y);

        // スプライトの左右反転処理
        if (Mathf.Abs(rb.velocity.x) > 0.01f) // 横移動速度がわずかでもある場合
        {
            GetComponent<SpriteRenderer>().flipX = rb.velocity.x < 0;
        }
    }

    // 地面に接地しているかを判定する
    private bool isGrounded()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.BoxCast(c.bounds.center, c.bounds.size, 0f, Vector2.down, 0.1f, GroundLayer);
        return hit.collider != null;
    }

    // プレイヤーの接触処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        // クリア用オブジェクトとの接触
        if (obj.CompareTag("Frag"))
        {
            Debug.Log("旗と接触しました");
            SceneManager.LoadScene("Clear_Scene");
        }

        //落下死等の即死系
        if (obj.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene("Result_Scene");
        }

        // 通常の敵との接触処理
        if (obj.CompareTag("Enemy"))
        {
            HitEnemy(obj);
        }

        // パタパタとの接触処理
        if (obj.CompareTag("Patapata"))
        {
            HITPata(obj);
        }
    }

    // 敵を踏んだり接触した際の処理
    private void HitEnemy(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();

        // プレイヤーの底辺が敵の上辺よりも上の場合
        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("敵を踏みつけました: " + enemy.name);
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // 敵を落下させる処理
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.bodyType = RigidbodyType2D.Dynamic; // 物理演算を有効化
                enemyRb.gravityScale = 1.5f; // 重力を調整
            }

            // プレイヤーをバウンドさせる
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc(); // プレイヤーがダメージを受ける
            Debug.Log("敵に接触してダメージを受けました");
        }
    }

    // プレイヤーの体力計算
    private void PlayerHpCalc()
    {
        playerHp -= 30;
        Debug.Log("現在の体力: " + playerHp);
        if (playerHp <= 0)
        {
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void HITPata(GameObject enemy)
    {
        Bounds playerBounds = GetComponent<BoxCollider2D>().bounds;
        Bounds enemyBounds = enemy.GetComponent<BoxCollider2D>().bounds;

        if (playerBounds.min.y > enemyBounds.max.y)
        {
            Debug.Log("敵を踏みつけました: " + enemy.name);

            // パタパタの落下処理を呼び出す
            PatapataMovement patapataMovement = enemy.GetComponent<PatapataMovement>();
            if (patapataMovement != null)
            {
                patapataMovement.StompedDown(gameObject); // プレイヤー自身を引数として渡す
            }

            // プレイヤーをバウンドさせる
            rb.velocity = new Vector2(rb.velocity.x, bounceForce);
        }
        else
        {
            PlayerHpCalc(); // プレイヤーがダメージを受ける
            Debug.Log("敵に接触してダメージを受けました");
        }
    }

    public void SaveCurrentStage()
    {
        string currentStage = SceneManager.GetActiveScene().name;
        RestartGame.SetLastStage(currentStage);
        NextStage.SetNextStage(currentStage);
    }

    // アイテムとの接触処理
    public string targetTag = "GrowItem";

    private void OnTriggerEnter(Collider other)
    {
        // アイテムタグを確認
        if (other.CompareTag(targetTag))
        {
            // プレイヤーのサイズを大きくする
            transform.localScale *= 5.0f;
        }
    }
}
