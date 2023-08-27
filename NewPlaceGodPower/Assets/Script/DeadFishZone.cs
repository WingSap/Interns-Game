using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadFishZone : MonoBehaviour
{
    public GameObject FishOBJ;

    private void LateUpdate()
    {
        FishOBJ = GameObject.FindGameObjectWithTag("Fish");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Fish"))
        {
            Destroy(FishOBJ);
        }
    }
}
