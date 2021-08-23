using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] TowerTemplate[] towerTemplate;
    [SerializeField] EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����
    [SerializeField] PlayerGold playerGold; // Ÿ�� �Ǽ� �� ��� ���Ҹ� ����..
    [SerializeField] SystemTextViewer systemTextViewer;
    bool isOnTowerButton;
    GameObject followTowerClone;
    int towerType;

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;

        if (isOnTowerButton)
        {
            return;
        }

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        // Ÿ���� �Ǽ��� ��ŭ ���� ������ Ÿ�� �Ǽ� x
        if(towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            // ��尡 �����ؼ� Ÿ�� �Ǽ��� �Ұ����ϴٰ� ���
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        // Ÿ�� �Ǽ� ��ư�� �����ٰ� ����
        isOnTowerButton = true;
        // ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        // Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        // Ÿ�� �Ǽ� ��ư�� ������ ���� Ÿ�� �Ǽ� ����
        if (!isOnTowerButton) return;

        Tile tile = tileTransform.transform.GetComponent<Tile>();

        // Ÿ�� �Ǽ� ���� ���� Ȯ��
        if (tile.IsBuildTower)
        {
            systemTextViewer.PrintText(SystemType.Build);
        }
        else
        {
            // �ٽ� Ÿ�� �Ǽ� ��ư�� ������ Ÿ���� �Ǽ��ϵ��� ���� ����
            isOnTowerButton = false;
            // Ÿ���� �Ǽ��Ǿ� �������� ����
            tile.IsBuildTower = true;
            // Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
            playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
            // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�(Ÿ�Ϻ��� z�� -1�� ��ġ�� ��ġ)
            Vector3 position = tileTransform.position + Vector3.back;
            GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
            // Ÿ�� ���⿡ enemySpawner ���� ����
            clone.GetComponent<TowerWeapon>().Setup(this, enemySpawner, playerGold, tile);

            // ���� ��ġ�Ǵ� Ÿ���� ���� Ÿ�� �ֺ��� ��ġ�� ���
            // ���� ȿ���� ���� �� �ֵ��� ��� ���� Ÿ���� ���� ȿ�� ����
            OnBuffAllBuffTowers();

            // Ÿ���� ��ġ�߱� ������ ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
            Destroy(followTowerClone);
            // Ÿ�� �Ǽ��� ����� �� �ִ� �ڷ�ƾ �Լ� ����
            StopCoroutine("OnTowerCancelSystem");
        }
    }

    IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            // ESC�� �Ǵ� ���콺 ������ ��ư�� ������ �� Ÿ�� �Ǽ� ���
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
            {
                isOnTowerButton = false;
                // ���콺�� ����ٴϴ� �ӽ� Ÿ�� ����
                Destroy(followTowerClone);
                break;
            }

            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i = 0; i<towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if(weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}