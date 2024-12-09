using UnityEngine;

public class big : MonoBehaviour
{
    public float speed = 5f; // オブジェクトが動く速さ

    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    
        gameObject.transform.position = new Vector3(100, 100, 100);

    }

}


