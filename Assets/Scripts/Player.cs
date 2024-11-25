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
    public float bounceForce = 10f; // 踏みつけ後のジャンプ力
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

    // デバッグ用：オブジェクトの階層構造を表示
    private void PrintObjectHierarchy(GameObject obj, string indent = "")
    {
        string info = $"{indent}Object: {obj.name}, Tag: {obj.tag}, Layer: {LayerMask.LayerToName(obj.layer)}";
        Debug.Log(info);

        // 親オブジェクトの情報
        if (obj.transform.parent != null)
        {
            Debug.Log($"{indent}Parent: {obj.transform.parent.name}");
        }

        // コンポーネント情報
        Component[] components = obj.GetComponents<Component>();
        foreach (Component comp in components)
        {
            Debug.Log($"{indent}Component: {comp.GetType().Name}");
        }

        // 子オブジェクトを再帰的に表示
        foreach (Transform child in obj.transform)
        {
            PrintObjectHierarchy(child.gameObject, indent + "  ");
        }


    } //ここまででバック用

    private void OnCollisionEnter2D_(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            // 死亡したらリザルトシーンに移動する
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;  // 既に死亡処理中なら無視

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 衝突した位置を確認
            Vector2 hitPoint = collision.contacts[0].point;
            Vector2 playerBottom = new Vector2(transform.position.x, GetComponent<Collider2D>().bounds.min.y);

            // プレイヤーの足元から少し上までを踏みつけ判定とする
            float stompThreshold = 0.1f; // 適宜調整してください

            if (rb.velocity.y < 0 && hitPoint.y < (transform.position.y - stompThreshold))
            {
                // 踏みつけ成功
                HandleEnemyStomped(collision.gameObject);
            }
            else
            {
                // 敵に当たってダメージ
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
        // 敵を無効化
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
        if (enemySprite != null)
        {
            enemySprite.enabled = false;
        }

        // コライダーを無効化
        Collider2D[] colliders = enemy.GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        // 跳ね返り
        rb.velocity = new Vector2(rb.velocity.x, bounceForce);

        // 遅延して敵を完全に削除
        StartCoroutine(DelayedDestroyEnemy(enemy));
    }

    private void HandlePlayerDeath()
    {
        if (isDead) return;

        isDead = true;
        // 死亡アニメーションなどを追加できます
        StartCoroutine(DelayedLoadResult());
    }

    private IEnumerator DelayedLoadResult()
    {
        // 少し待ってからシーン遷移（必要に応じて調整）
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Result_Scene");
    }

    private IEnumerator DelayedDestroyEnemy(GameObject enemy)
    {
        Debug.Log($"Destroy target: {enemy.name}");
        yield return new WaitForSeconds(0.5f);

        if (enemy != null && enemy.CompareTag("Enemy")) // タグが正しいか再確認
        {
            Destroy(enemy);
        }
        else
        {
            Debug.LogWarning("Destroy skipped: Object is null or not tagged as 'Enemy'");
        }
    }


    // シーン開始時の初期化
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



