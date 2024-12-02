using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class kurisinu : MonoBehaviour
{
    public class PlayerStomp : MonoBehaviour
    {
        public float bounceForce = 5f; // �v���C���[�����˂��

        void OnTriggerEnter2D(Collider2D collision)
        {
            // �G��"Enemy"�^�O���t���Ă���ꍇ�ɏ���
            if (collision.CompareTag("Enemy"))
            {
                // �G������
                Destroy(collision.gameObject);

                // �v���C���[�𒵂˕Ԃ�
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
