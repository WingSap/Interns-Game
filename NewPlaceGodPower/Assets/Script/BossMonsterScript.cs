using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterScript : MonoBehaviour
{
    [Header("Boss hp")]
    [SerializeField] private int bossMonsterMaxHp;
    public int bossMonsterCurrentHp;
    [Space]

    [Header("Boss movement")]
    [SerializeField] private float Movement;

    /*[Header("Flash color")]
    public SpriteRenderer spriteRenderer;
    public Color flashColor = Color.white;
    private float flashDuration = 0.1f;
    private int flashCount = 2;
    private Color originalColor;
    [Space]*/

    [Header("Shooter")]
    [SerializeField] private float coolDown;
    [SerializeField] private Transform ShootPoint;

    [Space]

    [Header("Instance Monster")]
    [SerializeField] private GameObject monsterPfab_1;
    [SerializeField] private GameObject monsterPfab_2;
    [SerializeField] private GameObject monsterPfab_3;

    private void Awake()
    {
        /*if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        originalColor = spriteRenderer.color;*/
    }

    public void BossTakeDamage(int Dmg)
    {

    }
}
