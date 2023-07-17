using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SocialPlatforms.Impl;

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

    [SerializeField] private float cooldownTime = 1.0f;

    [SerializeField] private float cooldownGroupTime = 10.0F;
    float currentTime = 0.0f; 

    bool canShoot = true; 

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

        if (Input.GetMouseButton(0) && canShoot)
        {
            Shoot();
            StartCooldown();
        }

        if (!canShoot && currentTime < cooldownTime)
        {
            currentTime += Time.deltaTime;
        }

        if (!canShoot && currentTime >= cooldownTime)
        {
            canShoot = true;
        }

        if (WaveSpawner.score >= 500)
        {
            _isActiveBullet = true;
            BulletSkill1.SetActive(true);
            BulletSkill2.SetActive(true);
        }

        if (WaveSpawner.score >= 1000)
        {
            _isActiveGroup = true;
            GroupBullet.SetActive(true);
        }
    }

    void Shoot()
    {
        canShoot = false;
        currentTime = 0.0f; 
        GameObject bullet = Instantiate(bulletPrefab, BulletPoint.position, BulletPoint.rotation);

        if(_isActiveBullet)
        {
            GameObject bullet1 = Instantiate(bulletPrefab, BulletPoint2.position, BulletPoint2.rotation);
            GameObject bullet2 = Instantiate(bulletPrefab, BulletPoint3.position, BulletPoint3.rotation);
        }
    }

    void GroupBulletShoot()
    {

        if (WaveSpawner.score > 1500)
        {
            cooldownGroupTime = 3.0F;
        }

        if (WaveSpawner.score > 3000)
        {
            cooldownGroupTime = 3.0F;
        }

        currentGroupTime = 0.0f;

        if (_isActiveGroup)
        {
            GameObject GroupBulletObj = Instantiate(PfabGroupBullet, GroupBulletPosition.position, GroupBulletPosition.rotation);
        }
    }

    void StartCooldown()
    {
        canShoot = false;
        currentTime = 0.0f;
    }
}
