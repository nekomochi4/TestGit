using UnityEngine;

public class big : MonoBehaviour
{
    public float speed = 5f; // �I�u�W�F�N�g����������

    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    
        gameObject.transform.position = new Vector3(100, 100, 100);

    }

}


