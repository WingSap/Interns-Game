using UnityEngine;
using System.IO;

public class _newImportImage : MonoBehaviour
{
    [Header("Render Camera")]
    [SerializeField] private RenderTexture _RT;
    [SerializeField] private GameObject _Camera;
    [Space]

    [Header("File Name")]
    [SerializeField] private string fileNamePrefix;
    [Space]

    [Header("Save to")]
    //E.X. --> C:\Users\DClub-15\Desktop\77\
    [SerializeField] private string saveFolderPath;

    [Header("Test Press K")]
    [SerializeField] private bool _test;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.K) && _test)
        {
            Get_Imange();
        }
    }

    public void Get_Imange()
    {
        _Camera.SetActive(true);
        Texture2D _texture2D = new Texture2D(_RT.width, _RT.height);

        RenderTexture.active = _RT;
        _texture2D.ReadPixels(new Rect(0, 0, _RT.width, _RT.height), 0, 0);
        _texture2D.Apply();


        string fileName = fileNamePrefix + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        //string _path = Application.persistentDataPath + "/" + _FileName + "" + _time + ".png";
        string filePath = saveFolderPath + fileName;
        byte[] _byte = _texture2D.EncodeToPNG();
        File.WriteAllBytes(filePath, _byte);

        Debug.Log(filePath);
    }
}
