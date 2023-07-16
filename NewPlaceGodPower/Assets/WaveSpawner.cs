using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab ของมอนสเตอร์
    public int waveCount = 3; // จำนวน Wave ที่ต้องการ
    public float spawnDelay = 0.25f; // เวลาระหว่างการเสกตัวละครแต่ละตัวใน Wave
    public float waveDelay = 2f; // เวลาระหว่างการเริ่ม Wave ใหม่

    private int currentWave = 0; // Wave ปัจจุบัน
    private int enemiesRemaining; // จำนวนมอนสเตอร์ที่เหลือใน Wave ปัจจุบัน
    private bool spawningWave; // สถานะการเสก Wave

    void Start()
    {
        enemiesRemaining = waveCount;
        spawningWave = false;
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        enemiesRemaining = waveCount;

        spawningWave = true;
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < waveCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }

        spawningWave = false;

        // เริ่ม Wave ใหม่หลังจากมอนสุดท้ายใน Wave ก่อนหน้านั้นตาย
        if (currentWave < waveCount)
        {
            yield return new WaitForSeconds(waveDelay);
            StartNextWave();
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemiesRemaining--;
    }

    void Update()
    {
        // ตรวจสอบสถานะการเสก Wave
        if (!spawningWave && enemiesRemaining == 0)
        {
            // การเสก Wave ถัดไป
            StartNextWave();
        }
    }
}
