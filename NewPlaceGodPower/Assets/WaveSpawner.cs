using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab �ͧ�͹�����
    public int waveCount = 3; // �ӹǹ Wave ����ͧ���
    public float spawnDelay = 0.25f; // ���������ҧ����ʡ����Ф����е��� Wave
    public float waveDelay = 2f; // ���������ҧ�������� Wave ����

    private int currentWave = 0; // Wave �Ѩ�غѹ
    private int enemiesRemaining; // �ӹǹ�͹������������� Wave �Ѩ�غѹ
    private bool spawningWave; // ʶҹС���ʡ Wave

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

        // ����� Wave ������ѧ�ҡ�͹�ش����� Wave ��͹˹�ҹ�鹵��
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
        // ��Ǩ�ͺʶҹС���ʡ Wave
        if (!spawningWave && enemiesRemaining == 0)
        {
            // ����ʡ Wave �Ѵ�
            StartNextWave();
        }
    }
}
