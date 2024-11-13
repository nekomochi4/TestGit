using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //UIを使うときに書きます。

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            // 死亡したらリザルトシーンに移動する
            SceneManager.LoadScene("Result_Scene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        // フラグアイテムに接触した場合、クリアシーンに移動する
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
