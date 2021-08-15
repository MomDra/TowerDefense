using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab; // 적 프리팹
    [SerializeField] float spawnTime; // 적 생성 주기
    [SerializeField] Transform[] wayPoints; // 현재 스테이지의 이동 경로

    private void Awake()
    {
        // 적 생성 코루틴 함수 호출
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
