using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform BulletPoint;

    [SerializeField] private float cooldownTime = 1.0f; // เวลาคูลดาว (ในวินาที)
    float currentTime = 0.0f; // เวลาปัจจุบันที่ผ่านไปตั้งแต่ยิงครั้งก่อนหน้า

    bool canShoot = true; // สถานะการยิง (true = สามารถยิงได้, false = ยังไม่สามารถยิงได้)

    private void Update()
    {
        if (WaveSpawner._isEndGame)
            return;

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
    }

    void Shoot()
    {
        // กำหนดให้ไม่สามารถยิงได้ในเบื้องต้น (จะเริ่มนับเวลาคูลดาวใหม่)
        canShoot = false;
        currentTime = 0.0f; // รีเซ็ตเวลาปัจจุบันเป็นศูนย์
        GameObject bullet = Instantiate(bulletPrefab, BulletPoint.position, BulletPoint.rotation);
    }

    void StartCooldown()
    {
        // เริ่มต้นการนับเวลาคูลดาว
        canShoot = false;
        currentTime = 0.0f;
    }
}
