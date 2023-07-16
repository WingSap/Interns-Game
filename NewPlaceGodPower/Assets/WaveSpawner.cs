using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public GameObject monsterPrefab1; // เลือก Prefab ของมอนสเตอร์ที่ต้องการสร้าง
    public GameObject monsterPrefab2;

    public int waveCount = 5; // จำนวน Wave ที่ต้องการสร้าง
    private float spawnDelay = 0f; // เวลาห่างระหว่างการเกิด Wave
    private float minSpawnDistance = 0f; // ระยะเกิดของมอนสเตอร์ (ระยะต่ำสุด)
    public float maxSpawnDistance = 5f; // ระยะเกิดของมอนสเตอร์ (ระยะสูงสุด)

    [SerializeField] private int currentWave = 0;

    [SerializeField] private GameObject ShowRetryUI;
    [SerializeField] private TextMeshProUGUI ShowText;
    private string Message;

    [SerializeField] private PlayerMovement Player;

    public static int score = 0;
    public TextMeshProUGUI scoreText;

    public static bool _isEndGame { get; private set; }

    IEnumerator Start()
    {
        _isEndGame = false;

        scoreText.text = "Score: " + score.ToString();

        ShowRetryUI.SetActive(false);

        while (currentWave < waveCount)
        {
            yield return StartCoroutine(SpawnWaveMonster());
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnWaveMonster()
    {
        if(!TimeToPlay.PlayTime)
        {

            currentWave++;

            if(currentWave <= 2)
            {
                for (int i = 0; i < currentWave * 10; i++)
                {
                    float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
                    Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;
                    SpawnMonster(spawnPosition , monsterPrefab1);

                    yield return new WaitForSeconds(0.2f); // เวลาห่างระหว่างการเกิด Wave
                }
            }
            else if (currentWave <= 20)
            {
                var wave2 = (currentWave * 10) / 5;

                for (int i = 0; i < currentWave * 10; i++)
                {
                    float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
                    Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;
                    SpawnMonster(spawnPosition, monsterPrefab1);

                    yield return new WaitForSeconds(0.2f); // เวลาห่างระหว่างการเกิด Wave
                }

                for (int j = 0; j < wave2; j++)
                {
                    float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
                    Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;
                    SpawnMonster(spawnPosition, monsterPrefab2);

                    yield return new WaitForSeconds(0.2f); // เวลาห่างระหว่างการเกิด Wave
                }
            }

            // รอให้มอนสเตอร์ใน Wave ปัจจุบันตายก่อนที่จะไปสร้าง Wave ต่อไป
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);

            if (currentWave == waveCount && GameObject.FindGameObjectsWithTag("Bot").Length == 0)
            {
                _isEndGame = true;
                Message = "You Win!!";
                ShowText.text = Message;
                ShowRetryUI.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f); // เวลาห่างระหว่างการเกิด Wave

        }

    }

    void SpawnMonster(Vector2 spawnPosition , GameObject Pfab)
    {
        // สร้างมอนสเตอร์ที่ตำแหน่งที่กำหนด
        GameObject monster = Instantiate(Pfab, spawnPosition, Quaternion.identity);
        BotHP bot = monster.GetComponent<BotHP>();

        bot.MonsterDeath += AddScore;
    }

    void OnDrawGizmos()
    {
        // วาด Gizmo สำหรับแสดงตำแหน่งเกิดของมอนสเตอร์
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
    }

    public void RetryButton()
    {
        score = 0;
        _isEndGame = false;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Player.PlayerCurrentHP <= 0)
        {
            _isEndGame = true;
            Message = "You Lose!!";
            ShowText.text = Message;
            ShowRetryUI.SetActive(true);
        }
    }

    void AddScore()
    {
        GameObject Bot = GameObject.FindGameObjectWithTag("Bot");
        BotHP botHP = Bot.GetComponent<BotHP>();
        score += botHP.Score;

        scoreText.text = "Score: " + score.ToString();
    }
}
