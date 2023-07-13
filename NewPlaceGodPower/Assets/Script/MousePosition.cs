using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private void Update()
    {
        // หาตำแหน่งของเมาส์ในโลกสกีน
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // ตั้งตำแหน่งวัตถุให้เท่ากับตำแหน่งเมาส์
        transform.position = mousePosition;
    }
}
