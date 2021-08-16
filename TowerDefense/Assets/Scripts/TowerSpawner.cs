using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] int towerBuildGold = 50; // 타워 건설에 사용되는 골드
    [SerializeField] EnemySpawner enemySpawner; // 현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    [SerializeField] PlayerGold playerGold; // 타워 건설 시 골드 감소를 위해..

    public void SpawnTower(Transform tileTransform)
    {
        // 타워 건설 가능 여부 확인
        // 1. 타워를 건설할 만큼 돈이 없으면 타워 건설 X
        if(towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }

        Tile tile = tileTransform.transform.GetComponent<Tile>();

        // 타워 건설 가능 여부 확인
        if (!tile.IsBuildTower)
        {
            // 타워가 건설되어 있음으로 설정
            tile.IsBuildTower = true;
            // 타워 건설에 필요한 골드만큼 감소
            playerGold.CurrentGold -= towerBuildGold;
            // 선택한 타일의 위치에 타워 건설(타일보다 z축 -1의 위치에 배치)
            Vector3 position = tileTransform.position + Vector3.back;
            GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
            // 타워 무기에 enemySpawner 정보 전달
            clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
        }
    }
}
