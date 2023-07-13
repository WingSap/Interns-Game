using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeToPlay : MonoBehaviour
{
    [SerializeField] private GameObject PlayUI;

    public static bool PlayTime;

    private void Awake()
    {
        PlayUI.SetActive(true);
        PlayTime = true;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            PlayTime = false;
            PlayUI.SetActive(false);
        }
    }
}
