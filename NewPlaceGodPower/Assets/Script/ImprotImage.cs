using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImprotImage : MonoBehaviour
{
    [SerializeField] private RenderTexture _RT;
    [SerializeField] private GameObject _Camera;
    [SerializeField] private string _FileName;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Get_Imange();
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
        _Camera.SetActive(false);
        File.WriteAllBytes(_path, _byte);

        Debug.Log(Application.persistentDataPath);
    }
}
