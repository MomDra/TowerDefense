using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPlayerHP;
    [SerializeField] TextMeshProUGUI textPlayerGold;
    [SerializeField] PlayerHP playerHP;
    [SerializeField] PlayerGold playerGold;

    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
    }
}
