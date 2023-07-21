using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDMGZone : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageInterval = 0.5f;

    private bool canDamage = true;

    public float Ctime = 0.5F;

    private void Update()
    {
        Ctime -= Time.deltaTime;
        if (Ctime < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bot"))
        {
            canDamage = true;

            StartCoroutine(ApplyDamage(other.GetComponent<Bot>()));
        }
    }

    private IEnumerator ApplyDamage(Bot botHP)
    {
        while (canDamage)
        {
            botHP.BotTakeDMG(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bot"))
        {
            canDamage = false;
            StopCoroutine(ApplyDamage(other.GetComponent<Bot>()));
        }
    }
}
