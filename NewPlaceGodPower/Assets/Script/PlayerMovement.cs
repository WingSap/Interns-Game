using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [SerializeField] private Rigidbody2D rb;
    private bool canDash = true;

    private Transform PlayerMouse;

    public int PlayerHP;
    public int PlayerCurrentHP;
    public Slider slider;

    private float iFrame;
    [SerializeField] private float setiFrame;

    public SpriteRenderer spriteRenderer; // Reference เพื่อเปลี่ยนสี Sprite
    public Color flashColor = Color.white; // สีที่ใช้ในการกระพิบ
    public float flashDuration = 0.1f; // ระยะเวลาในการกระพิบ
    public int flashCount = 3; // จำนวนครั้งที่กระพิบ

    private Color originalColor; // สีเดิมของ Sprite

    public TextMeshProUGUI playerHpText;

    public GameObject PlayerSprite;

    public Transform DeadPosition;
    public GameObject DeadPfab;
    public int Deadcount;

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
        Deadcount = 5;
        PlayerSprite.SetActive(true);
        PlayerCurrentHP = PlayerHP;
        slider.maxValue = PlayerHP;
        slider.value = PlayerHP;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        iFrame -= Time.deltaTime;

        if (TimeToPlay.PlayTime)
            return;

        if (WaveSpawner._isEndGame)
        {
            if (PlayerCurrentHP <= 0 || PlayerCurrentHP == 0)
            {
                PlayerSprite.SetActive(false);
                if(Deadcount == 5)
                {
                    GameObject Dead = Instantiate(DeadPfab, DeadPosition.position, DeadPosition.rotation);
                    Deadcount = 6;
                }
                PlayerCurrentHP = 0;
            }
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        // ดึงตำแหน่งเมาส์ในหน้าจอ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // ตั้งค่า z เป็น 0 เพื่อให้วัตถุอยู่ในระดับเดียวกับหน้าจอ 2D

        // คำนวณการหมุนของวัตถุตามตำแหน่งของเมาส์
        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // หมุนวัตถุในแกน Z
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // รับค่าเคลื่อนที่จากผู้เล่นในแนวระนาบ
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // เคลื่อนที่ตามแนวระนาบ
        Vector2 movement = new Vector2(moveX, moveY);
        rb.velocity = movement * moveSpeed;

        // ทำการ Dash เมื่อกดปุ่ม Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;

        // เก็บค่าความเร็วเดิม
        float previousSpeed = moveSpeed;

        // เพิ่มความเร็วของการ Dash
        moveSpeed = dashSpeed;

        // รอเวลาจนกว่าการ Dash จะสิ้นสุดลง
        yield return new WaitForSeconds(dashDuration);

        // คืนค่าความเร็วเดิม
        moveSpeed = previousSpeed;

        // รอเวลาในการเติมพลังงาน Dash
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;

        iFrame = setiFrame;
    }

    private void OnGUI()
    {
        playerHpText.text = "Player HP : " + PlayerCurrentHP.ToString();
    }

    public void playerTakeDmg(int Dmg)
    {
        if(iFrame <= 0)
        {
            PlayerCurrentHP -= Dmg;
            slider.value = PlayerCurrentHP;
            StartCoroutine(Flash());
        }
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
}
