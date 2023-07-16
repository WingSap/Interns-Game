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

    private float bounciness = 0f; // �������Ǣͧ��á����
    private float friction = 0f; // ����������·���Դ�������ͪ��ѹ

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

        // �֧���˹�������˹�Ҩ�
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // ��駤�� z �� 0 ��������ѵ��������дѺ���ǡѺ˹�Ҩ� 2D

        // �ӹǳ�����ع�ͧ�ѵ�ص�����˹觢ͧ�����
        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // ��ع�ѵ���᡹ Z
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // �Ѻ�������͹���ҡ����������йҺ
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // ����͹��������йҺ
        Vector2 movement = new Vector2(moveX, moveY);
        rb.velocity = movement * moveSpeed;

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
