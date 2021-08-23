using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField] TowerTemplate[] towerTemplate;
    [SerializeField] EnemySpawner enemySpawner; // 현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    [SerializeField] PlayerGold playerGold; // 타워 건설 시 골드 감소를 위해..
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

        // 타워 건설 가능 여부 확인
        // 타워를 건설할 만큼 돈이 없으면 타워 건설 x
        if(towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            // 골드가 부족해서 타워 건설이 불가능하다고 출력
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        // 타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;
        // 마우스를 따라다니는 임시 타워 생성
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);
        // 타워 건설을 취소할 수 있는 코루틴 함수 시작
        StartCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        // 타워 건설 버튼을 눌렀을 때만 타워 건설 가능
        if (!isOnTowerButton) return;

        Tile tile = tileTransform.transform.GetComponent<Tile>();

        // 타워 건설 가능 여부 확인
        if (tile.IsBuildTower)
        {
            systemTextViewer.PrintText(SystemType.Build);
        }
        else
        {
            // 다시 타워 건설 버튼을 눌러서 타워를 건설하도록 변수 설정
            isOnTowerButton = false;
            // 타워가 건설되어 있음으로 설정
            tile.IsBuildTower = true;
            // 타워 건설에 필요한 골드만큼 감소
            playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
            // 선택한 타일의 위치에 타워 건설(타일보다 z축 -1의 위치에 배치)
            Vector3 position = tileTransform.position + Vector3.back;
            GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
            // 타워 무기에 enemySpawner 정보 전달
            clone.GetComponent<TowerWeapon>().Setup(this, enemySpawner, playerGold, tile);

            // 새로 배치되는 타워가 버프 타워 주변에 배치될 경우
            // 버프 효과를 받을 수 있도록 모든 버프 타워의 버프 효과 갱신
            OnBuffAllBuffTowers();

            // 타워를 배치했기 때문에 마우스를 따라다니는 임시 타워 삭제
            Destroy(followTowerClone);
            // 타워 건설을 취소할 수 있는 코루틴 함수 중지
            StopCoroutine("OnTowerCancelSystem");
        }
    }

    IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            // ESC가 또는 마우스 오른쪽 버튼을 눌렀을 때 타워 건설 취소
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1))
            {
                isOnTowerButton = false;
                // 마우스를 따라다니는 임시 타워 삭제
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