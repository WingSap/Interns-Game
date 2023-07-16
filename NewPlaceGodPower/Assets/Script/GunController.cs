using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform BulletPoint;
    public Transform BulletPoint2;
    public Transform BulletPoint3;

    public GameObject BulletSkill1;
    public GameObject BulletSkill2;

    public GameObject GroupBullet;
    public Transform GroupBulletPosition;
    public GameObject PfabGroupBullet;

    [SerializeField] private bool _isActiveBullet;
    [SerializeField] private bool _isActiveGroup;

    [SerializeField] private float cooldownTime = 1.0f; // เวลาคูลดาว (ในวินาที)

    [SerializeField] private float cooldownGroupTime = 10.0F;
    float currentTime = 0.0f; // เวลาปัจจุบันที่ผ่านไปตั้งแต่ยิงครั้งก่อนหน้า

    bool canShoot = true; // สถานะการยิง (true = สามารถยิงได้, false = ยังไม่สามารถยิงได้)

    bool canGroupShoot = true;

    float currentGroupTime = 0.0f;

    private void Awake()
    {
        BulletSkill1.SetActive(false);
        BulletSkill2.SetActive(false);

        GroupBullet.SetActive(false);

    }

    private void Update()
    {
        if (WaveSpawner._isEndGame)
            return;

        currentGroupTime += Time.deltaTime;

        if (_isActiveGroup)
        {
            if (currentGroupTime >= cooldownGroupTime)
            {
                GroupBulletShoot();
            }

        }

        // ตรวจสอบการยิงเมื่อกดปุ่มซ้ายเมาส์
        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
            StartCooldown(); // เริ่มต้นการนับเวลาคูลดาว
        }

        // หากยังไม่สามารถยิงได้และเวลาคูลดาวยังไม่ครบ
        if (!canShoot && currentTime < cooldownTime)
        {
            currentTime += Time.deltaTime; // เพิ่มเวลาที่ผ่านไปตั้งแต่ครั้งก่อนหน้า
        }

        // หากเวลาคูลดาวครบแล้ว
        if (!canShoot && currentTime >= cooldownTime)
        {
            canShoot = true; // สามารถยิงได้อีกครั้งแล้ว
        }

        if (_isActiveBullet)
        {
            BulletSkill1.SetActive(true);
            BulletSkill2.SetActive(true);
        }

        if (_isActiveGroup)
        {
            GroupBullet.SetActive(true);
        }
    }

    void Shoot()
    {
        // กำหนดให้ไม่สามารถยิงได้ในเบื้องต้น (จะเริ่มนับเวลาคูลดาวใหม่)
        canShoot = false;
        currentTime = 0.0f; // รีเซ็ตเวลาปัจจุบันเป็นศูนย์
        GameObject bullet = Instantiate(bulletPrefab, BulletPoint.position, BulletPoint.rotation);

        if(_isActiveBullet)
        {
            GameObject bullet1 = Instantiate(bulletPrefab, BulletPoint2.position, BulletPoint2.rotation);
            GameObject bullet2 = Instantiate(bulletPrefab, BulletPoint3.position, BulletPoint3.rotation);
        }
    }

    void GroupBulletShoot()
    {
        currentGroupTime = 0.0f;

        if (_isActiveGroup)
        {
            GameObject GroupBulletObj = Instantiate(PfabGroupBullet, GroupBulletPosition.position, GroupBulletPosition.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BulletPack"))
        {
            _isActiveBullet = true;
        }

        if (collision.gameObject.CompareTag("GroupBulletPack"))
        {
            _isActiveGroup = true;
        }

    }

    void StartCooldown()
    {
        // เริ่มต้นการนับเวลาคูลดาว
        canShoot = false;
        currentTime = 0.0f;
    }

    void StartGroupCooldown()
    {
        canGroupShoot = false;
        currentGroupTime = 0.0F;
    }
}
