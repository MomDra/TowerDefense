using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textPlayerHP;
    [SerializeField] TextMeshProUGUI textPlayerGold;
    [SerializeField] TextMeshProUGUI textWave;
    [SerializeField] PlayerHP playerHP;
    [SerializeField] PlayerGold playerGold;
    [SerializeField] WaveSystem waveSystem;

    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
    }
}
