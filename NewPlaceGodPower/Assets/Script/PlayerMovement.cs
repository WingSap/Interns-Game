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

    private Rigidbody2D rb;
    private bool canDash = true;

    private Transform PlayerMouse;

    public int PlayerHP;
    public int PlayerCurrentHP;
    public Slider slider;

    private float bounciness = 0f; // ความเร็วของการกระเด็น
    private float friction = 0f; // ความเสียหายที่เกิดขึ้นเมื่อชนกัน

    private float iFrame;
    [SerializeField] private float setiFrame;

    public TextMeshProUGUI playerHpText;
    private void Start()
    {
        PlayerCurrentHP = PlayerHP;
        slider.maxValue = PlayerHP;
        slider.value = PlayerHP;

        rb = GetComponent<Rigidbody2D>();

        rb.sharedMaterial.bounciness = bounciness;
        rb.sharedMaterial.friction = friction;
    }

    private void Update()
    {
        iFrame -= Time.deltaTime;

        if (TimeToPlay.PlayTime)
            return;

        if (TimeSetting._isEndGame)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            return;
        }

        if(PlayerCurrentHP <= 0 || PlayerCurrentHP == 0)
        {
            PlayerCurrentHP = 0;
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
        }
    }
}
