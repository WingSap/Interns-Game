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

    public TextMeshProUGUI playerHpText;
    private void Start()
    {
        PlayerCurrentHP = PlayerHP;
        slider.maxValue = PlayerHP;
        slider.value = PlayerHP;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (TimeToPlay.PlayTime)
            return;

        // รับค่าเคลื่อนที่จากผู้เล่นในแนวระนาบ
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // เคลื่อนที่ตามแนวระนาบ
        Vector2 movement = new Vector2(moveX, moveY);
        rb.velocity = movement * moveSpeed;

        PlayerMouse = GameObject.FindGameObjectWithTag("PlayerMouse").transform;

        // หันหน้าตามตำแหน่งของผู้เล่น
        Vector2 direction = PlayerMouse.position - transform.position;
        transform.right = direction;

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
    }

    private void OnGUI()
    {
        playerHpText.text = "Player HP : " + PlayerCurrentHP.ToString();
    }

    public void playerTakeDmg(int Dmg)
    {
        if(canDash)
        {
            PlayerCurrentHP -= Dmg;
            slider.value = PlayerCurrentHP;
        }
    }
}
