using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Movement2D movement2D;
    Transform target;

    public void Setup(Transform target)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
    }

    private void Update()
    {
        if (target)
        {
            // �߻�ü�� target�� ��ġ�� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; // ���� �ƴ� ���� �ε�����
        if (collision.transform != target) return; // ���� target�� ���� �ƴ� ��

        collision.GetComponent<Enemy>().OnDie();
        Destroy(gameObject);
    }
}
