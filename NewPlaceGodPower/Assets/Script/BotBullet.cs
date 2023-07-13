using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBullet : MonoBehaviour
{
    public float moveSpeed = 10f;

    public float TimeC = 1.0F;

    public GameObject PotionPfab;
    public Transform ThisgameOBJ;

    private void Update()
    {
        TimeC -= Time.deltaTime;
        if (TimeC > 0)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if (TimeC < 0)
        {
            AddDMGFild();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "wall")
        {
            AddDMGFild();
        }

        if (collision.tag == "Player")
        {
            AddDMGFild();
        }

    }

    private void AddDMGFild()
    {
        GameObject PS = Instantiate(PotionPfab, ThisgameOBJ.position, ThisgameOBJ.rotation);
        Destroy(this.gameObject);
    }
}
