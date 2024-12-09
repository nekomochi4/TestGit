using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject itemPrefab; // 出現するアイテムのプレハブ
    public Transform spawnPoint; // アイテムの出現位置
    private bool isUsed = false; // ブロックが既に使用されたか

    void OnCollisionEnter(Collision collision)
    {
        // プレイヤーが下からブロックを叩いたかを確認
        if (!isUsed && collision.contacts[0].normal.y < -0.5f)
        {
            isUsed = true;

            // ブロックを少し揺らすアニメーション
            StartCoroutine(BlockHitAnimation());

            // アイテムを生成
            if (itemPrefab != null && spawnPoint != null)
            {
                Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator BlockHitAnimation()
    {
        Vector3 originalPosition = transform.position;

        // ブロックを上に少し動かす
        transform.position += Vector3.up * 0.2f;
        yield return new WaitForSeconds(0.1f);

        // 元の位置に戻す
        transform.position = originalPosition;
    }
}
