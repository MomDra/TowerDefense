using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.transform.GetComponent<Tile>();

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        if (!tile.IsBuildTower)
        {
            // Ÿ���� �Ǽ��Ǿ� �������� ����
            tile.IsBuildTower = true;

            // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
            GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
            // Ÿ�� ���⿡ enemySpawner ���� ����
            clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
        }
    }
}
