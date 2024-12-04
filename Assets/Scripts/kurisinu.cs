using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class kurisinu : MonoBehaviour
{
    public class PlayerStomp : MonoBehaviour
    {
        public float bounceForce = 5f; // プレイヤーが跳ねる力

        void OnTriggerEnter2D(Collider2D collision)
        {
            // 敵に"Enemy"タグが付いている場合に処理
            if (collision.CompareTag("Enemy"))
            {
                // 敵を消す
                Destroy(collision.gameObject);

                // プレイヤーを跳ね返す
                Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = new Vector2(rb.velocity.x, bounceForce);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
