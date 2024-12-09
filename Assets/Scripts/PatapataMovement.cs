using UnityEngine;

public class PatapataMovement : MonoBehaviour
{
    public float moveSpeed = 2f;  // 移動速度
    public float moveHeight = 3f; // 移動の高さ

    private Rigidbody2D rb;
    private Collider2D col;
    private Vector2 startPosition;
    private bool canMove = true;

    void Start()
    {
        // Rigidbody2Dコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();

        // Collider2Dコンポーネントを取得
        col = GetComponent<Collider2D>();

        // 初期位置を保存
        startPosition = rb.position;

        // 重力の影響を無効にする
        rb.gravityScale = 0;
    }

    void FixedUpdate()
    {
        // canMoveがtrueの場合のみ上下に移動
        if (canMove)
        {
            // Sin関数を使って上下の位置を計算
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;

            // 新しい位置を設定（X, Zは変更しない）
            rb.MovePosition(new Vector2(startPosition.x, newY));
        }
    }

    // プレイヤーに踏みつけられた時の処理
    public void StompedDown(GameObject enemy)
    {
        canMove = false; // 移動を停止
        rb.bodyType = RigidbodyType2D.Dynamic; // 重力を有効化
        rb.gravityScale = 1.5f; // 落下するように重力を設定

        // 当たり判定を無効化
        if (col != null)
        {
            col.enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        // 当たり判定のギズモを表示
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
