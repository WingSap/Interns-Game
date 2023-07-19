using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class _newImportImage : MonoBehaviour
{
    [SerializeField] private RenderTexture _RT;
    [SerializeField] private GameObject _Camera;
    [SerializeField] private string _FileName;

    [SerializeField] private GameObject[] renderItem;

    private void Awake()
    {
        for (int i = 0; i < renderItem.Length; i++)
        {
            renderItem[i].SetActive(false);
        }
    }

    private void Update()
    {

        for (int i = 0; i < renderItem.Length; i++)
        {
            int targetInt = (i+1) * 1000;

            if (i == 0 && WaveSpawner.score >= 1000 && WaveSpawner.score <= 1001)
            {
                StartCoroutine(delay(i));
            }

            else if(WaveSpawner.score == targetInt)
            {
                StartCoroutine(delay(i));
            }
        }
    }
    private void Get_Imange()
    {
        _Camera.SetActive(true);
        Texture2D _texture2D = new Texture2D(_RT.width, _RT.height);

        RenderTexture.active = _RT;
        _texture2D.ReadPixels(new Rect(0, 0, _RT.width, _RT.height), 0, 0);
        _texture2D.Apply();
        string _time = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        string _path = Application.persistentDataPath + "/" + _FileName + "" + _time + ".png";
        byte[] _byte = _texture2D.EncodeToPNG();
        File.WriteAllBytes(_path, _byte);

        Debug.Log(Application.persistentDataPath);
    }

    IEnumerator delay(int i)
    {
        int SetTrue = i;
        int SetFalse = i - 1;
        int targetInt = (i + 1) * 1000;

        if (i == 0)
        {
            renderItem[i].SetActive(true);
            Get_Imange();
        }
        else if(WaveSpawner.score == targetInt)
        {
            renderItem[SetTrue].SetActive(true);
            renderItem[SetFalse].SetActive(false);
            Get_Imange();
        }

        yield return new WaitForSeconds(0.1F);
    }
}
