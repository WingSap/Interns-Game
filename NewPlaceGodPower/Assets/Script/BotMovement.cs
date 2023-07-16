using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 5f;
    public float shootingInterval = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private Transform player;
    private float shootingTimer;

    public GameObject[] itemPrefabs; // รายการของไอเท็มที่ต้องการสุ่มดรอป
    public float dropChance = 0.1f; // โอกาศในการดรอปเป็นเปอร์เซ็นต์ (10%)

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (TimeToPlay.PlayTime)
            return;

        if (WaveSpawner._isEndGame)
            return;

        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            Vector2 direction = player.position - transform.position;
            transform.right = direction;

            if (Vector2.Distance(transform.position, player.position) <= stoppingDistance)
            {
                shootingTimer -= Time.deltaTime;
                if (shootingTimer <= 0)
                {
                    Shoot();
                    shootingTimer = shootingInterval;
                }
            }
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
