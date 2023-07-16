using System;
using UnityEngine;
using System.Collections;


public class BotHP : MonoBehaviour
{
    public int BotHp;

    public int BotCurrentHP;

    public event Action MonsterDeath;

    public SpriteRenderer spriteRenderer; // Reference เพื่อเปลี่ยนสี Sprite
    public Color flashColor = Color.white; // สีที่ใช้ในการกระพิบ
    public float flashDuration = 0.1f; // ระยะเวลาในการกระพิบ
    public int flashCount = 3; // จำนวนครั้งที่กระพิบ

    private Color originalColor; // สีเดิมของ Sprite

    public int Score;

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
        BotCurrentHP = BotHp;
    }

    private void Update()
    {
        if (BotCurrentHP <= 0 || BotCurrentHP == 0)
        {
            BotCurrentHP = 0;
            MonsterDeath?.Invoke();
            Destroy(this.gameObject);
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
}

