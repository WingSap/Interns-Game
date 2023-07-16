using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // เลือก Prefab ของมอนสเตอร์ที่ต้องการสร้าง
    public int waveCount = 5; // จำนวน Wave ที่ต้องการสร้าง
    private float spawnDelay = 0f; // เวลาห่างระหว่างการเกิด Wave
    private float minSpawnDistance = 0f; // ระยะเกิดของมอนสเตอร์ (ระยะต่ำสุด)
    public float maxSpawnDistance = 5f; // ระยะเกิดของมอนสเตอร์ (ระยะสูงสุด)

    [SerializeField] private int currentWave = 0;

    [SerializeField] private GameObject ShowRetryUI;
    [SerializeField] private TextMeshProUGUI ShowText;
    private string Message;

    [SerializeField] private PlayerMovement Player;

    private int score = 0;
    public TextMeshProUGUI scoreText;

    public static bool _isEndGame { get; private set; }

    IEnumerator Start()
    {
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

            for (int i = 0; i < currentWave * 10; i++)
            {
                float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
                Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;
                SpawnMonster(spawnPosition);

                yield return new WaitForSeconds(0.2f); // เวลาห่างระหว่างการเกิด Wave
            }

            // รอให้มอนสเตอร์ใน Wave ปัจจุบันตายก่อนที่จะไปสร้าง Wave ต่อไป
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);

            if (currentWave == waveCount && GameObject.FindGameObjectsWithTag("Bot").Length == 0)
            {
                Message = "You Win!!";
                ShowText.text = Message;
                ShowRetryUI.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f); // เวลาห่างระหว่างการเกิด Wave

        }

    }

    void SpawnMonster(Vector2 spawnPosition)
    {
        // สร้างมอนสเตอร์ที่ตำแหน่งที่กำหนด
        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
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
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Player.PlayerCurrentHP <= 0)
        {
            Message = "You Lose!!";
            ShowText.text = Message;
            ShowRetryUI.SetActive(true);
        }
    }

    void AddScore()
    {
        score += 10;
        scoreText.text = "Score: " + score.ToString();
    }
}
