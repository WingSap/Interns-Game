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

        // �Ѻ�������͹���ҡ����������йҺ
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // ����͹��������йҺ
        Vector2 movement = new Vector2(moveX, moveY);
        rb.velocity = movement * moveSpeed;

        PlayerMouse = GameObject.FindGameObjectWithTag("PlayerMouse").transform;

        // �ѹ˹�ҵ�����˹觢ͧ������
        Vector2 direction = PlayerMouse.position - transform.position;
        transform.right = direction;

        // �ӡ�� Dash ����͡����� Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    private IEnumerator Dash()
    {
        canDash = false;

        // �纤�Ҥ����������
        float previousSpeed = moveSpeed;

        // �����������Ǣͧ��� Dash
        moveSpeed = dashSpeed;

        // �����Ҩ����ҡ�� Dash ������شŧ
        yield return new WaitForSeconds(dashDuration);

        // �׹��Ҥ����������
        moveSpeed = previousSpeed;

        // ������㹡�������ѧ�ҹ Dash
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
