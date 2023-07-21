using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    public int BotHp;

    public int BotCurrentHP;

    public event Action MonsterDeath;

    public SpriteRenderer spriteRenderer; // Reference ��������¹�� Sprite
    public Color flashColor = Color.white; // �շ����㹡�á�оԺ
    public float flashDuration = 0.1f; // ��������㹡�á�оԺ
    public int flashCount = 3; // �ӹǹ���駷���оԺ

    private Color originalColor; // ������ͧ Sprite

    public int Score;

    public GameObject DeadPfab;
    public Transform DeadPosition;

    public float moveSpeed = 3f;
    private Transform player;
    [SerializeField] private int DMG;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        originalColor = spriteRenderer.color;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        BotCurrentHP = BotHp;
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

        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            Vector2 direction = player.position - transform.position;
            transform.right = direction;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement PlayerHp = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerHp.playerTakeDmg(DMG);
            Ondead();
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
