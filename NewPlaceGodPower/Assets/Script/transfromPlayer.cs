using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transfromPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 lookDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // ในที่นี้เราใช้นิ้วแรกที่สัมผัสจอ

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position); // แปลงตำแหน่งของนิ้วในจอเป็นตำแหน่งในโลก 2D
            touchPosition.z = 0;

            Vector3 TouchlookDirection = touchPosition - transform.position;
            float Touchangle = Mathf.Atan2(TouchlookDirection.y, TouchlookDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(Touchangle - 90, Vector3.forward);
            // สามารถเพิ่มโค้ดที่ควบคุมการหมุนของวัตถุตามตำแหน่งนิ้วที่นี่
        }

        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
