using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject itemPrefab; // �o������A�C�e���̃v���n�u
    public Transform spawnPoint; // �A�C�e���̏o���ʒu
    private bool isUsed = false; // �u���b�N�����Ɏg�p���ꂽ��

    void OnCollisionEnter(Collision collision)
    {
        // �v���C���[��������u���b�N��@���������m�F
        if (!isUsed && collision.contacts[0].normal.y < -0.5f)
        {
            isUsed = true;

            // �u���b�N�������h�炷�A�j���[�V����
            StartCoroutine(BlockHitAnimation());

            // �A�C�e���𐶐�
            if (itemPrefab != null && spawnPoint != null)
            {
                Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }

    private IEnumerator BlockHitAnimation()
    {
        Vector3 originalPosition = transform.position;

        // �u���b�N����ɏ���������
        transform.position += Vector3.up * 0.2f;
        yield return new WaitForSeconds(0.1f);

        // ���̈ʒu�ɖ߂�
        transform.position = originalPosition;
    }
}
