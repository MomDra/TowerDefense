using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPAlpha : MonoBehaviour
{
    [SerializeField] float lerpTime = 0.5f;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void FadeOut()
    {
        StartCoroutine(AlphaLerp(1f, 0f));
    }

    IEnumerator AlphaLerp(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while(percent < 1)
        {
            // lerpTime �ð����� while() �ݺ��� ����
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;

            // Text - TextMeshPro�� ��Ʈ ������ start���� end�� ����
            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;

            yield return null;
        }
    }
}
