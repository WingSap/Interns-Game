using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotHP : MonoBehaviour
{
    public int BotHp;

    public int BotCurrentHP;

    private void Start()
    {
        BotCurrentHP = BotHp;
    }

    private void Update()
    {
        if(BotCurrentHP <= 0 || BotCurrentHP == 0)
        {
            BotCurrentHP = 0;
        }
    }

    public void BotTakeDMG(int Dmg)
    {
        BotCurrentHP -= Dmg;
    }
}
