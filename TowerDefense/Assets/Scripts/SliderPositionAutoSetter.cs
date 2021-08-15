using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField] Vector3 distance = Vector3.down * 20.0f;
    Transform targetTransform;
    RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider UI�� �i�ƴٴ� target ����
        targetTransform = target;
        // RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // ���� �ı��Ǿ� �i�ƴٴ� ����� ������� Slider UI�� ����
        if (!targetTransform)
        {
            Destroy(gameObject);
            return;
        }

        // ������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        // LateUpdate()���� ȣ���Ѵ�.

        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // ȭ�� ������ ��ǥ + distance ��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distance;
    }
}
