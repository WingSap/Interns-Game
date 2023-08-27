using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public enum BotType
{
    NORNAL_BOT,
    MOVE_AROUND_PLAVER_BOT,
    LINE_MOVEMENT_RIGHT,
    LINE_MOVEMENT_LEFT,
}

public class Bot : MonoBehaviour
{
    [Header("Bot hp and score")]
    public int BotHp;
    public int BotCurrentHP;

    public int Score;
    public event Action MonsterDeath;
    [Space]

    [Header("Bot movement")]
    private Transform player;
    public float moveSpeed = 3f;
    [Space]

    [Header("Bot damage")]
    [SerializeField] private int DMG;
    [Space]

    [Header("Flash color")]
    public SpriteRenderer spriteRenderer;
    public Color flashColor = Color.white;
    private float flashDuration = 0.1f;
    private int flashCount = 3;
    private Color originalColor;
    [Space]

    [Header("BotDead")]
    public GameObject DeadPfab;
    public Transform DeadPosition;
    [Space]

    [Header("Rotation Obj (Bot v2)")]
    public float rotateSpeed = 75.0F; // ความเร็วในการหมุน
    private float circleRadius = 7.0F; // รัศมีของวงกลม
    private bool isMovingToTarget = true; // เก็บสถานะการเคลื่อนที่ไปหาเป้าหมาย
    private Vector2 initialPosition; // ตำแหน่งเริ่มต้นของวัตถุ
    [Space]

    [Header("Shoot")]
    [SerializeField] private Transform ShootPosition;
    [SerializeField] private GameObject BulletPfab;
    private float _currentTime = 0;
    [SerializeField] private float _coolDown = 0.25F;

    [Header("Bot Ver ?")]
    [SerializeField] private BotType _myTypeBot;

    private void Awake()
    {
        if(_myTypeBot == BotType.LINE_MOVEMENT_RIGHT)
        {
            this.gameObject.transform.Rotate(new Vector3(0 , 0 , 90));
        }

        else if(_myTypeBot == BotType.LINE_MOVEMENT_LEFT)
        {
            this.gameObject.transform.Rotate(new Vector3(0, 0, -90));
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalColor = spriteRenderer.color;
        initialPosition = transform.position;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        BotCurrentHP = BotHp;
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (TimeToPlay.PlayTime)
            return;

        if (WaveSpawner._isEndGame)
            return;

        if (BotCurrentHP <= 0 || BotCurrentHP == 0)
        {
            Ondead();
        }

        #region Normal Bot

        if (_myTypeBot == BotType.NORNAL_BOT)
        {
            if (player != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                Vector2 direction = player.position - transform.position;
                transform.right = direction;
            }
        }

        #endregion

        #region Move around player Bot

        else if (_myTypeBot == BotType.MOVE_AROUND_PLAVER_BOT)
        {
            _currentTime -= Time.deltaTime;
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > circleRadius && isMovingToTarget)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else
            {
               
                if (isMovingToTarget)
                {
                    isMovingToTarget = false;
                }
                
                Rotation();

                if(_currentTime <= 0.0F)
                {
                    StartCoroutine(delayShoot());
                    _currentTime = _coolDown;
                }
            }
        }

        #endregion

        #region Line movement Bot Left & Right

        else if (_myTypeBot == BotType.LINE_MOVEMENT_RIGHT)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += moveSpeed * Time.deltaTime;
            transform.position = currentPosition;
        }

        else if (_myTypeBot == BotType.LINE_MOVEMENT_LEFT)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x -= moveSpeed * Time.deltaTime;
            transform.position = currentPosition;
        }

        if(_myTypeBot == BotType.LINE_MOVEMENT_LEFT || _myTypeBot == BotType.LINE_MOVEMENT_RIGHT)
        {
            _currentTime -= Time.deltaTime;

            if (_currentTime <= 0.0F)
            {
                StartCoroutine(Shoot());
                _currentTime = _coolDown;
            }
        }

        #endregion

    }

    private IEnumerator Shoot()
    {
        GameObject Bullet = Instantiate(BulletPfab, ShootPosition.position, ShootPosition.rotation);
        yield return new WaitForSeconds(0.25F);
    }

    private IEnumerator delayShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject Bullet = Instantiate(BulletPfab, ShootPosition.position, ShootPosition.rotation);
            yield return new WaitForSeconds(0.25F);
        }

    }

    private void Rotation()
    {
        transform.RotateAround(player.position, Vector3.forward, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement PlayerHp = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerHp.playerTakeDmg(DMG);
            Ondead();
        }
        else if(_myTypeBot == BotType.LINE_MOVEMENT_LEFT || _myTypeBot == BotType.LINE_MOVEMENT_RIGHT)
        {
            if(collision.gameObject.CompareTag("wall"))
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void BotTakeDMG(int Dmg)
    {
        BotCurrentHP -= Dmg;
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void Ondead()
    {
        BotCurrentHP = 0;
        MonsterDeath?.Invoke();
        GameObject dead = Instantiate(DeadPfab, DeadPosition.position, DeadPosition.rotation);
        Destroy(this.gameObject);
    }
}
