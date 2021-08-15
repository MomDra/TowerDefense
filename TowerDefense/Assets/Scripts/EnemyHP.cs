using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] float maxHP;
    float currentHP;
    bool isDie = false;
    Enemy enemy;
    SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // Tip. 적의 체력이 damage 만큼 감소해서 죽을 상황일 때 여러 타워의 공격을 동사에 받으면
        // enemy.OnDie() 함수가 여러 번 실행될 수 있다.

        // 현재 적의 상태가 사망 상태이면 아래 코드를 실행하지 않는다.
        if (!isDie)
        {
            // 현재 체력을 damage만큼 감소
            currentHP -= damage;

            StopCoroutine("HitAlphaAnimation");
            StartCoroutine("HitAlphaAnimation");

            // 체력이 0이하 = 적 캐릭터 사망
            if (currentHP <= 0)
            {
                isDie = true;
                // 적 캐릭터 사망
                enemy.OnDie();
            }
        }
    }

    IEnumerator HitAlphaAnimation()
    {
        // 현재 적의 색상을 color 변수에 저장
        Color color = spriteRenderer.color;

        // 적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05초 동안 대기
        yield return new WaitForSeconds(0.05f);

        // 적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
