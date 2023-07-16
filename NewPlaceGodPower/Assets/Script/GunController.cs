using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform BulletPoint;

    [SerializeField] private float cooldownTime = 1.0f; // ���Ҥ�Ŵ�� (��Թҷ�)
    float currentTime = 0.0f; // ���һѨ�غѹ����ҹ仵�����ԧ���駡�͹˹��

    bool canShoot = true; // ʶҹС���ԧ (true = ����ö�ԧ��, false = �ѧ�������ö�ԧ��)

    private void Update()
    {
        if (WaveSpawner._isEndGame)
            return;

        // ��Ǩ�ͺ����ԧ����͡��������������
        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
            StartCooldown(); // ������鹡�ùѺ���Ҥ�Ŵ��
        }

        // �ҡ�ѧ�������ö�ԧ��������Ҥ�Ŵ���ѧ���ú
        if (!canShoot && currentTime < cooldownTime)
        {
            currentTime += Time.deltaTime; // �������ҷ���ҹ仵������駡�͹˹��
        }

        // �ҡ���Ҥ�Ŵ�Ǥú����
        if (!canShoot && currentTime >= cooldownTime)
        {
            canShoot = true; // ����ö�ԧ���ա��������
        }
    }

    void Shoot()
    {
        // ��˹�����������ö�ԧ������ͧ�� (��������Ѻ���Ҥ�Ŵ������)
        canShoot = false;
        currentTime = 0.0f; // �������һѨ�غѹ���ٹ��
        GameObject bullet = Instantiate(bulletPrefab, BulletPoint.position, BulletPoint.rotation);
    }

    void StartCooldown()
    {
        // ������鹡�ùѺ���Ҥ�Ŵ��
        canShoot = false;
        currentTime = 0.0f;
    }
}
