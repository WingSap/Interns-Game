using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    private void Update()
    {
        // �ҵ��˹觢ͧ�������šʡչ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // ��駵��˹��ѵ�������ҡѺ���˹������
        transform.position = mousePosition;
    }
}
