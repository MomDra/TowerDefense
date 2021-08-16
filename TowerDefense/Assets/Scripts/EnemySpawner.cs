using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] GameObject enemyPrefab; // �� ������
    [SerializeField] GameObject enemyHPSliderPrefab; // �� ü���� ��Ÿ���� Slider UI ������
    [SerializeField] Transform canvasTransform; // UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField] float spawnTime; // �� ���� �ֱ�
    [SerializeField] Transform[] wayPoints; // ���� ���������� �̵� ���
    [SerializeField] PlayerHP playerHP; // �ÿ��̾��� ü�� ������Ʈ
    [SerializeField] PlayerGold playerGold; // �÷��̾��� ��� ������Ʈ
    Wave currentWave;
    List<Enemy> enemyList; // ���� �ʿ� �����ϴ� ��� ���� ����

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();

        // �� ���� �ڷ�ƾ �Լ� ȣ��
        //StartCoroutine(SpawnEnemy());
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        // ���� ���̺� ����
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        // ���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;

        // ���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ ����
        while(spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);

            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
        }

    }

    public void DestroyEnemy(EnemyDestoryType type, Enemy enemy, int gold)
    {
        if(type == EnemyDestoryType.Arrive)
        {
            playerHP.TakeDamage(1);
        }
        // ���� �÷��̾��� �߻�ü���� ������� ��
        else if(type == EnemyDestoryType.Kill)
        {
            playerGold.CurrentGold += gold;
        }

        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);

        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }

    void SpawnEnemyHPSlider(GameObject enemy)
    {
        // �� ü���� ��Ÿ���� Slider UI ����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        // Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        // Tip. UI�� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ�
        sliderClone.transform.SetParent(canvasTransform);
        // ���� �������� �ٲ� ũ�⸦ �ٽ� (1, 1, 1)�� ����
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI�� �i�ƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
