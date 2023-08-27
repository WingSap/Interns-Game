using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WaveSpawnerMonster
{
    public int _NormalBot;
    public int _LeftRightBot ;
    public int _MoveAroundBot;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private WaveSpawnerMonster[] waveSpawnerMonsters;

    [Header("Can next wave")]
    public static bool _canNextWave;

    [Header("Monster Pfab")]
    public GameObject NormalBot; 
    public GameObject LeftBot;
    public GameObject RightBot;
    public GameObject MoveAroundBot;
    [Space]

    [Header("Spawner bot Left Right Position")]
    [SerializeField] private Transform _positionRight;
    [SerializeField] private Transform _positionLeft;
    [SerializeField] private bool _spawnerRightBot = true;

    [Header("Wave Spawner")]
    public int maxWaveSpawner = 5;
    [SerializeField] private int currentWave = 1;
    private float spawnDelay = 0f;

    [Header("Spawner Distance")]
    public float minSpawnDistance = 0f;
    public float maxSpawnDistance = 5f;
    [Space]

    [Header("Score UI and text")]
    public static int score = 0;
    public TextMeshProUGUI scoreText;
    [SerializeField] private GameObject ShowRetryUI;
    [SerializeField] private GameObject ShowSaveImageUI;
    [SerializeField] private TextMeshProUGUI ShowText;
    private string Message;
    [Space]

    [Header("Player Dead")]
    [Space]
    [SerializeField] private PlayerMovement PlayerIsDead;
    public static bool _isEndGame { get; private set; }

    IEnumerator Start()
    {
        maxWaveSpawner = waveSpawnerMonsters.Length;
        _isEndGame = false;
        _canNextWave = true;
        scoreText.text = "Score: " + score.ToString();

        ShowRetryUI.SetActive(false);
        ShowSaveImageUI.SetActive(false);

        while (currentWave < maxWaveSpawner)
        {
            yield return StartCoroutine(SpawnWaveMonster());
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnWaveMonster()
    {
        if (!TimeToPlay.PlayTime && _canNextWave)
        {
            for (int i = 0; i < waveSpawnerMonsters.Length; i++)
            {
                yield return new WaitUntil(() => _canNextWave);

                StartCoroutine(spawnerNormalBot(waveSpawnerMonsters[i]._NormalBot));
                StartCoroutine(spawnerBotLeftRight(waveSpawnerMonsters[i]._LeftRightBot));
                StartCoroutine(spawnerMoveAroundBot(waveSpawnerMonsters[i]._MoveAroundBot));

                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);
                currentWave = i;
                StartCoroutine(Delay());
            }

            #region Wave Spawner

            /*currentWave++;

            if (currentWave <= maxWaveSpawner)
            {
                
                if(currentWave == 1)
                {
                    StartCoroutine(spawnerNormalBot(currentWave));
                    StartCoroutine(spawnerBotLeftRight(currentWave));


                    yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);
                    StartCoroutine(Delay());

                }

                else if (currentWave == 2)
                {
                    StartCoroutine(spawnerNormalBot(currentWave));
                    yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);
                    StartCoroutine(Delay());
                }

                else if (currentWave == 3)
                {
                    StartCoroutine(spawnerNormalBot(currentWave));
                    yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);
                    StartCoroutine(Delay());
                }

                else if (currentWave == 4)
                {
                    StartCoroutine(spawnerMoveAroundBot(currentWave));
                    yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);
                    StartCoroutine(Delay());
                }

                else if (currentWave == 5)
                {
                    StartCoroutine(spawnerNormalBot(currentWave));
                    StartCoroutine(spawnerBotLeftRight(currentWave));
                    StartCoroutine(spawnerMoveAroundBot(currentWave));

                    yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);
                    StartCoroutine(Delay());
                }
                

            }
            */

            #endregion

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Bot").Length == 0);

            yield return new WaitForSeconds(0.5f);

        }

    }

    IEnumerator spawnerNormalBot(int currentMonster)
    {
        for (int i = 0; i < currentMonster; i++)
        {
            float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;

            SpawnMonster(spawnPosition, NormalBot);
            yield return new WaitForSeconds(0.2f);
        }

    }

    IEnumerator spawnerBotLeftRight(int currentMonster)
    {
        for (int i = 0; i < currentMonster; i++)
        {
            if (_spawnerRightBot)
            {
                Vector2 spawnPosition = _positionRight.transform.position;

                SpawnMonster(spawnPosition, LeftBot);

                _spawnerRightBot = !_spawnerRightBot;

                yield return new WaitForSeconds(1f);
            }
            else if (!_spawnerRightBot)
            {
                Vector2 spawnPosition = _positionLeft.transform.position;

                SpawnMonster(spawnPosition, RightBot);

                _spawnerRightBot = !_spawnerRightBot;

                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator spawnerMoveAroundBot(int currentMonster)
    {
        for (int i = 0; i < currentMonster; i++)
        {
            float spawnDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector2 spawnPosition = Random.insideUnitCircle.normalized * spawnDistance;

            SpawnMonster(spawnPosition, MoveAroundBot);

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void winCheck()
    {
        if (currentWave +1 == waveSpawnerMonsters.Length)
        {
            _isEndGame = true;
            Message = "You Win!!";
            ShowText.text = Message;
            ShowRetryUI.SetActive(true);
        }
        else
            return;
    }

    void SpawnMonster(Vector2 spawnPosition , GameObject Pfab)
    {
        GameObject monster = Instantiate(Pfab, spawnPosition, Quaternion.identity);
        Bot bot = monster.GetComponent<Bot>();

        bot.MonsterDeath += AddScore;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxSpawnDistance);
        Gizmos.DrawWireSphere(transform.position, minSpawnDistance);
    }

    public void RetryButton()
    {
        score = 0;
        _isEndGame = false;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (PlayerIsDead.PlayerCurrentHP <= 0)
        {
            _isEndGame = true;
            Message = "You Lose!!";
            ShowText.text = Message;
            ShowRetryUI.SetActive(true);
        }
    }

    IEnumerator Delay()
    {
        if (currentWave == waveSpawnerMonsters.Length -1)
        {
            _isEndGame = true;
            Message = "You Win!!";
            ShowText.text = Message;
            ShowRetryUI.SetActive(true);
            yield return new WaitForSeconds(0.5F);
        }
        else if (PlayerIsDead.PlayerCurrentHP <= 0)
        {
            yield return null;
        }
        else if(!_isEndGame)
        {
            _canNextWave = false;
            yield return new WaitForSeconds(0.5F);
            ShowSaveImageUI.SetActive(true);
        }
    }

    public void SetTrue()
    {
        GunController._playerCanShoot = true;
        _canNextWave = true;
    }

    void AddScore()
    {
        GameObject Bot = GameObject.FindGameObjectWithTag("Bot");
        Bot botHP = Bot.GetComponent<Bot>();

        score += botHP.Score;

        scoreText.text = "Score: " + score.ToString();
    }
}
