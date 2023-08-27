using UnityEngine;

public class BulletBotHit : MonoBehaviour
{
    [Header("Damage and movement")]
    public float moveSpeed = 0.0f;
    [SerializeField] private int BulletDMG = 10;
    [Space]

    [SerializeField] private float Addmovement = 3.0F;

    public Transform target; // เป้าหมายที่ควรหมุนไปยัง
    public float rotationSpeed = 200f;

    [Header("Destory Bullet")]
    [SerializeField] private GameObject HitPfab;
    [SerializeField] private Transform instanceHitAtThisGameOBJ;

    private void Awake()
    {
        moveSpeed = 0.0F;
        FindTarget();
    }

    private void FindTarget()
    {
        // หา GameObject ที่มีแท็กตรงกับ targetTag
        GameObject targetObject = GameObject.FindGameObjectWithTag("Player");
        if (targetObject != null)
        {
            target = targetObject.transform;
        }
    }

    private void Update()
    {
        Addmovement -= Time.deltaTime;
        if(Addmovement <= 0.0F)
        {
            HitSomething();
        }

        if (target != null && Addmovement <= 3.0F && Addmovement >= 1.0F)
        {
            Vector2 directionToTarget = target.position - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            moveSpeed = 1.5F;
        }

        else if(Addmovement <= 1.0F)
        {
            moveSpeed = 15.0F;
        }

        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {
            HitSomething();
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            player.playerTakeDmg(BulletDMG);
        }
    }

    private void HitSomething()
    {
        GameObject PS = Instantiate(HitPfab, instanceHitAtThisGameOBJ.position, instanceHitAtThisGameOBJ.rotation);
        Destroy(this.gameObject);
    }
}
