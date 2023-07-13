using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeSetting : MonoBehaviour
{
    [SerializeField] private float sec;
    [SerializeField] private TextMeshProUGUI Timeetxt;
    [SerializeField] private TextMeshProUGUI TextMessage;

    [SerializeField] private GameObject UIRetry;
    [SerializeField] private string Message;

    [SerializeField] private BotHP botHP;
    [SerializeField] private PlayerMovement playerHp;

    public GameObject GroupbulletPrefab; 
    public Transform[] spawnPoints;

    [SerializeField] private GameObject playerGroupBullet;
    [SerializeField] private GameObject BotAiGroupBullet;

    [SerializeField] private float Cooldown;

    public bool IsPlay;

    private void Start()
    {
        sec = 90.0F;

        UIRetry.SetActive(false);

        IsPlay = true;
    }

    private void OnGUI()
    {
        Timeetxt.text = sec.ToString("F0");
    }

    private void Update()
    {
        if (TimeToPlay.PlayTime)
            return;

        sec -= Time.deltaTime;

        Cooldown += Time.deltaTime;

        if (botHP.BotCurrentHP <= 0 || playerHp.PlayerCurrentHP <= 0)
        {
            ActiveUIRetry();
        }

        if (Cooldown >= 10.0F) 
        {
            randomskill();
        }

        if(sec <= 0)
        {
            ActiveUIRetry();
        }

    }

    private void ActiveUIRetry()
    {
        if(botHP.BotCurrentHP <= 0 || playerHp.PlayerCurrentHP > botHP.BotCurrentHP)
        {
            Message = "You Win!!";
            TextMessage.text = Message;
            UIRetry.SetActive(true);
        }
        else if (playerHp.PlayerCurrentHP <= 0 || playerHp.PlayerCurrentHP < botHP.BotCurrentHP)
        {
            Message = "You Loses!!";
            TextMessage.text = Message;
            UIRetry.SetActive(true);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    private void randomskill()
    {
        Cooldown = 0;

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        int randomNum = Random.Range(1, 3);


        if (randomNum == 1)
        {
            GameObject bullet = Instantiate(playerGroupBullet, spawnPoint.position, spawnPoint.rotation);
        }
        else if (randomNum == 2)
        {
            GameObject bullet = Instantiate(BotAiGroupBullet, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
