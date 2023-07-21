using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("Rigibody")]
    [Space]
    [SerializeField] private Rigidbody2D rb;
    [Space]

    [Header("Player Hp Setting")]
    public int PlayerHP;
    public int PlayerCurrentHP;
    public Slider slider;
    public TextMeshProUGUI playerHpText;
    public GameObject PlayerSprite;
    [Space]

    [Header("Flash color")]
    [Space]
    public SpriteRenderer spriteRenderer;
    public Color flashColor = Color.white;
    private float flashDuration = 0.1f;
    private int flashCount = 2;
    private Color originalColor;
    [Space]


    [Header("Player Dead")]
    [Space]
    public Transform DeadPosition;
    public GameObject DeadPfab;
    private bool _isPlayerDead;

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
        _isPlayerDead = false;
        PlayerSprite.SetActive(true);
        PlayerCurrentHP = PlayerHP;
        slider.maxValue = PlayerHP;
        slider.value = PlayerHP;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (TimeToPlay.PlayTime)
            return;

        if (WaveSpawner._isEndGame)
        {
            if (PlayerCurrentHP <= 0 || PlayerCurrentHP == 0 && !_isPlayerDead)
            {
                PlayerDead();
            }

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

    }

    private void OnGUI()
    {
        playerHpText.text = "Player HP : " + PlayerCurrentHP.ToString();
    }

    public void playerTakeDmg(int Dmg)
    {
        PlayerCurrentHP -= Dmg;
        slider.value = PlayerCurrentHP;
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void PlayerDead()
    {
        PlayerCurrentHP = 0;
        PlayerSprite.SetActive(false);
        GameObject Dead = Instantiate(DeadPfab, DeadPosition.position, DeadPosition.rotation);
        _isPlayerDead = true;
    }
}
