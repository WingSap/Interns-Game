using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPfab : MonoBehaviour
{
    [HideInInspector] [Header("Camera capscreen")]
    [Space]
    _newImportImage image;

    [Header("Movement and rotation")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 120.0F;
    [Space]

    [Header("Fish hp")]
    [SerializeField] private int HpFish;
    [Space]

    public float scaleAmount = 10f;

    [Header("Dead")]
    [SerializeField] private float DealyAnimation = 1.0F;
    public static bool _isFishDead;
    [Space]

    [Header("flash color")]
    public SpriteRenderer spriteRenderer;
    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;
    public int flashCount = 3;
    private Color originalColor;
    [Space]

    [Header("Paticale")]
    [SerializeField] private Transform _myTransform;
    [SerializeField] private GameObject _myPaticaleObj;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        originalColor = spriteRenderer.color;
        image = GameObject.Find("CapImageCamera").GetComponent<_newImportImage>();
    }


    private void Update()
    {
        RotateToDirection();

        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void RotateToDirection()
    {
        Vector3 direction = transform.position + (Vector3.right * moveSpeed * Time.deltaTime) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && HpFish > 0)
        {
            HpFish--;
            StartCoroutine(FlashCoroutine());
        }
        else if (Input.touchCount > 0 && HpFish > 0)
        {
            HpFish--;
            StartCoroutine(FlashCoroutine());
        }

        else if (HpFish <= 0)
        {
            _isFishDead = true;
            StartCoroutine(_isDead());
        }
    }

    IEnumerator _isDead()
    {
        GameObject _myPaticle = Instantiate(_myPaticaleObj, _myTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(DealyAnimation);
        Destroy(this.gameObject);
    }

    IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
