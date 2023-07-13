using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotHP : MonoBehaviour
{
    public int BotHp;
    public TextMeshProUGUI BotHpText;

    public int BotCurrentHP;
    public Slider slider;

    private void Start()
    {
        BotCurrentHP = BotHp;
        slider.maxValue = BotHp;
        slider.value = BotHp;
    }

    public void BotTakeDMG(int Dmg)
    {
        BotCurrentHP -= Dmg;
        slider.value = BotCurrentHP;
    }

    private void OnGUI()
    {
        BotHpText.text = "Bot HP : " + BotCurrentHP.ToString();
    }
}
