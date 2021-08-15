using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // �� ������
    [SerializeField] float spawnTime; // �� ���� �ֱ�
    [SerializeField] Transform[] wayPoints; // ���� ���������� �̵� ���

    private void Awake()
    {
        // �� ���� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(wayPoints);

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
