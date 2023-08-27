using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage and movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int BulletDMG = 10;
    [Space]

    [Header("Destory Bullet")]
    [SerializeField] private GameObject HitPfab;
    [SerializeField] private Transform instanceHitAtThisGameOBJ;
    [SerializeField] private float coolDown = 1.0F;

    private void Update()
    {
        coolDown -= Time.deltaTime;
        if (coolDown > 0)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        else if (coolDown < 0)
        {
            HitSomething();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bot")
        {
            Bot bot = collision.GetComponent<Bot>();
            bot.BotTakeDMG(BulletDMG);

            HitSomething();
        }
        else if(collision.tag == "BotBullet")
        {
            HitSomething();
        }

    }

    private void HitSomething()
    {
        GameObject PS = Instantiate(HitPfab, instanceHitAtThisGameOBJ.position, instanceHitAtThisGameOBJ.rotation);
        Destroy(this.gameObject);
    }
}
