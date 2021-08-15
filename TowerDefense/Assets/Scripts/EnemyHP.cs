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
        // Tip. ���� ü���� damage ��ŭ �����ؼ� ���� ��Ȳ�� �� ���� Ÿ���� ������ ���翡 ������
        // enemy.OnDie() �Լ��� ���� �� ����� �� �ִ�.

        // ���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 �������� �ʴ´�.
        if (!isDie)
        {
            // ���� ü���� damage��ŭ ����
            currentHP -= damage;

            StopCoroutine("HitAlphaAnimation");
            StartCoroutine("HitAlphaAnimation");

            // ü���� 0���� = �� ĳ���� ���
            if (currentHP <= 0)
            {
                isDie = true;
                // �� ĳ���� ���
                enemy.OnDie();
            }
        }
    }

    IEnumerator HitAlphaAnimation()
    {
        // ���� ���� ������ color ������ ����
        Color color = spriteRenderer.color;

        // ���� ������ 40%�� ����
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);

        // ���� ������ 100%�� ����
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
