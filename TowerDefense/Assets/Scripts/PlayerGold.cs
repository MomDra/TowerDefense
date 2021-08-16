using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField] int currentGold = 100;

    public int CurrentGold
    {
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }
}
