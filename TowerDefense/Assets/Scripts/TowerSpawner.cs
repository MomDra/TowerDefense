using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] int towerBuildGold = 50; // Ÿ�� �Ǽ��� ���Ǵ� ���
    [SerializeField] EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����
    [SerializeField] PlayerGold playerGold; // Ÿ�� �Ǽ� �� ��� ���Ҹ� ����..

    public void SpawnTower(Transform tileTransform)
    {
        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� X
        if(towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }

        Tile tile = tileTransform.transform.GetComponent<Tile>();

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        if (!tile.IsBuildTower)
        {
            // Ÿ���� �Ǽ��Ǿ� �������� ����
            tile.IsBuildTower = true;
            // Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
            playerGold.CurrentGold -= towerBuildGold;
            // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
            GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
            // Ÿ�� ���⿡ enemySpawner ���� ����
            clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
        }
    }
}
