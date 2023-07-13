using Unity.VisualScripting;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public GameObject bulletPrefab; // เลือกวัตถุกระสุนใน Inspector window

    private float angleStep;

    public float rotationSpeed = 900f; // ความเร็วในการหมุน

    private float Ctime = 2.0F;

    public int minBullets = 1; // จำนวนกระสุนขั้นต่ำที่แรนด้อม
    public int maxBullets = 5;
    public int numBullets;


    private void Start()
    {
        numBullets = Random.Range(minBullets, maxBullets + 1);
        angleStep = 360f / numBullets; // คำนวณค่ามุมสำหรับแต่ละกระสุน
    }

    private void Update()
    {
        Ctime -= Time.deltaTime;

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime); // หมุนวัตถุ
        if (Ctime <= 0)
        {
            for (int i = 0; i < numBullets; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // สร้างกระสุนใหม่
                bullet.transform.RotateAround(transform.position, Vector3.forward, angleStep * i); // หมุนกระสุนตามมุมที่กำหนด
            }

            Destroy(this.gameObject);
        }
    }
}
