using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        // ȭ���� ���콺 ��ǥ�� �������� ���� ���� ���� ��ǥ�� ���Ѵ�.
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = mainCamera.ScreenToWorldPoint(position);
        // z ��ġ�� 0���� ����
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
