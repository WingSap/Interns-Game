using Unity.VisualScripting;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public GameObject bulletPrefab; // ���͡�ѵ�ء���ع� Inspector window

    private float angleStep;

    public float rotationSpeed = 900f; // ��������㹡����ع

    private float Ctime = 2.0F;

    public int minBullets = 1; // �ӹǹ����ع��鹵�ӷ���ù����
    public int maxBullets = 5;
    public int numBullets;


    private void Start()
    {
        numBullets = Random.Range(minBullets, maxBullets + 1);
        angleStep = 360f / numBullets; // �ӹǳ����������Ѻ���С���ع
    }

    private void Update()
    {
        Ctime -= Time.deltaTime;

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime); // ��ع�ѵ��
        if (Ctime <= 0)
        {
            for (int i = 0; i < numBullets; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // ���ҧ����ع����
                bullet.transform.RotateAround(transform.position, Vector3.forward, angleStep * i); // ��ع����ع����������˹�
            }

            Destroy(this.gameObject);
        }
    }
}
