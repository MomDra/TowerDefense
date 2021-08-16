using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] TowerTemplate towerTemplate;
    [SerializeField] EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����
    [SerializeField] PlayerGold playerGold; // Ÿ�� �Ǽ� �� ��� ���Ҹ� ����..
    [SerializeField] SystemTextViewer systemTextViewer;
    public void SpawnTower(Transform tileTransform)
    {
        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // 1. Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� X
        if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        Tile tile = tileTransform.transform.GetComponent<Tile>();

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        if (tile.IsBuildTower)
        {
            systemTextViewer.PrintText(SystemType.Build);
        }
        else
        {
            // Ÿ���� �Ǽ��Ǿ� �������� ����
            tile.IsBuildTower = true;
            // Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
            playerGold.CurrentGold -= towerTemplate.weapon[0].cost;
            // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ��� z�� -1�� ��ġ�� ��ġ)
            Vector3 position = tileTransform.position + Vector3.back;
            GameObject clone = Instantiate(towerTemplate.towerPrefab, position, Quaternion.identity);
            // Ÿ�� ���⿡ enemySpawner ���� ����
            clone.GetComponent<TowerWeapon>().Setup(enemySpawner, playerGold);
        }
    }
}
