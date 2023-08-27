using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitANI : MonoBehaviour
{
    public Animator animator; // Reference to the animator component

    private bool isAnimationPlaying = false;

    /*[SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        // ���������������ǹ (ᴧ, ����, ����Թ) 㹪�ǧ 0-255
        int red = Random.Range(0, 256);
        int green = Random.Range(0, 256);
        int blue = Random.Range(0, 256);

        // ���ҧ�����������������
        Color randomColor = new Color(red / 255f, green / 255f, blue / 255f);

        // ��˹������Ѻ SpriteRenderer �ͧ GameObject
        this.spriteRenderer.color = randomColor;
    }*/

    private void Update()
    {
        // Check if the animation is currently playing
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            isAnimationPlaying = true;
        }
        else
        {
            isAnimationPlaying = false;
        }

        // Check if the animation has finished playing and destroy the object
        if (!isAnimationPlaying)
        {
            Destroy(gameObject);
        }
    }
}
