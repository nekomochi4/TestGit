using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2.0f;
    private bool isMovingRight = true; //falseやったら左に行く

    private void Update()
    {
        // 左右移動
        float direction = isMovingRight ? 1 : -1;
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 壁に当たったら向きを変える
        isMovingRight = !isMovingRight;
        FlipSprite();
    }

    private void FlipSprite()
    {
        // キャラクターの向きを反転
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}