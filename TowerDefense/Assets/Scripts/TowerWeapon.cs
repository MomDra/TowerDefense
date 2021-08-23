using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0, Laser, Slow, Buff, }
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser}

public class TowerWeapon : MonoBehaviour
{
    [Header("Commons")]
    [SerializeField] TowerTemplate towerTemplate;
    [SerializeField] WeaponType weaponType;
    [SerializeField] Transform spawnPoint;

    [Header("Cannon")]
    [SerializeField] GameObject projectilePrefab;

    [Header("Laser")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform hitEffect;
    [SerializeField] LayerMask targetLayer;

    int level = 0;
    WeaponState weaponState = WeaponState.SearchTarget;
    Transform attackTarget = null;
    EnemySpawner enemySpawner;
    SpriteRenderer spriteRenderer;
    TowerSpawner towerSpawner;
    PlayerGold playerGold;
    Tile ownerTile;

    float addedDamage;
    int buffLevel;

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    public WeaponType WeaponType => weaponType;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    public void Setup(TowerSpawner towerSpawner, EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        // ���� �Ӽ��� ĳ��, �������� ��
        if(weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            // ���� ���¸� WeaponState.SearchTarget���� ����
            ChangeState(WeaponState.SearchTarget);
        }
    }

    public void ChangeState(WeaponState newState)
    {
        // ������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        // ���� ����
        weaponState = newState;
        // ���ο� ���� ���
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if (attackTarget)
        {
            RotateToTarget();
        }
    }

    void RotateToTarget()
    {
        // �������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        // ���� = arctan(y/x)
        // x, y ������ ���ϱ�
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;
        // x, y �������� �������� ���� ���ϱ�
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    IEnumerator SearchTarget()
    {
        while (true)
        {
            attackTarget = FindClosestAttackTarget();

            if (attackTarget)
            {
                if(weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }
                else if(weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
                
            }

            yield return null;
        }
    }

    IEnumerator TryAttackCannon()
    {
        while (true)
        {
            if (!IsPossibleToAttackTarget())
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 3. attackRate �ð���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);

            // 4. ���� (�߻�ü ����)
            SpawnProjectile();
        }
    }

    IEnumerator TryAttackLaser()
    {
        // ������, ������ Ÿ�� ȿ�� Ȱ��ȭ
        EnableLaser();

        while (true)
        {
            // target�� �����ϴ°� �������� �˻�
            if (!IsPossibleToAttackTarget())
            {
                // ������, ������ Ÿ�� ȿ�� ��Ȱ��ȭ
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // ������ ����
            SpawnLaser();

            yield return null;
        }
    }

    public void OnBuffAroundTower()
    {
        // ���� �ʿ� ��ġ�� "Tower" �±׸� ���� ��� ������Ʈ Ž��
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i = 0; i< towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            // �̹� ������ �ް� �ְ�, ���� ���� Ÿ���� �������� ���� �����̸� �н�
            if(weapon.BuffLevel > Level)
            {
                continue;
            }

            // ���� ���� Ÿ���� �ٸ� Ÿ���� �Ÿ��� �˻��ؼ� ���� �ȿ� Ÿ���� ������
            if(Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                // ������ ������ ĳ��, ������ Ÿ���̸�
                if(weapon.WeaponType == WeaponType.Cannon || weapon.WeaponType == WeaponType.Laser)
                {
                    // ������ ���� ���ݷ� ����
                    weapon.AddedDamage = weapon.Damage * (towerTemplate.weapon[level].buff);
                    // Ÿ���� �ް� �ִ� ���� ���� ����
                    weapon.buffLevel = Level;
                }
            }
        }
    }

    Transform FindClosestAttackTarget()
    {
        // ���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;
        // EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
        for(int i =0; i < enemySpawner.EnemyList.Count; i++)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            // ���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
            if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
            }
        }

        return attackTarget;
    }

    bool IsPossibleToAttackTarget()
    {
        // target�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
        if (!attackTarget)
        {
            return false;
        }

        // target�� ���� ���� �ȿ� �ִ��� �˻� (���� ������ ����� ���ο� �� Ž��)
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if(distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }

    void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        // ������ �߻�ü���� ���ݴ��(attackTarget) ���� ����
        //clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);
        float damage = towerTemplate.weapon[level].damage + AddedDamage;
        clone.GetComponent<Projectile>().Setup(attackTarget, damage);
    }

    void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        // ���� �������� ���� ���� ������ ���� �� �� ����  attackTarget�� ������ ������Ʈ�� ����
        for(int i = 0; i<hit.Length; i++)
        {
            if (hit[i].transform == attackTarget)
            {
                // ���� ��������
                lineRenderer.SetPosition(0, spawnPoint.position);
                // ���� ��ǥ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                // Ÿ�� ȿ�� ��ġ ����
                hitEffect.position = hit[i].point;
                // �� ü�� ���� (1�ʿ� damage ��ŭ ����)
                //attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);
                float damage = towerTemplate.weapon[level].damage + AddedDamage;
                attackTarget.GetComponent<EnemyHP>().TakeDamage(damage * Time.deltaTime);
            }
        }
    }


    public bool Upgrade()
    {
        // Ÿ�� ���׷��̵忡 �ʿ��� ��尡 ������� �˻�
        if(playerGold.CurrentGold < towerTemplate.weapon[level + 1].cost)
        {
            return false;
        }

        // Ÿ�� ���� ����
        level++;
        // Ÿ�� ���� ���� (sprite)
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        // ��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        // ���� �Ӽ��� �������̸�
        if(weaponType == WeaponType.Laser)
        {
            // ������ ���� �������� ���� ����
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        // Ÿ���� ���׷��̵� �� �� ��� ���� Ÿ���� ���� ȿ�� ����
        // ���� Ÿ���� ���� Ÿ���� ���, ���� Ÿ���� ���� Ÿ���� ���
        towerSpawner.OnBuffAllBuffTowers();

        return true;
    }

    public void Sell()
    {
        // ��� ����
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        // ���� Ÿ�Ͽ� �ٽ� Ÿ�� �Ǽ��� �����ϵ��� ����
        ownerTile.IsBuildTower = false;
        // Ÿ�� �ı�
        Destroy(gameObject);
    }
}
