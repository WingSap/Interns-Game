using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR;

[System.Serializable]
public class GameData
{
    public string IP;
    public int DownloadImage;
}

public class SendImageToServer : MonoBehaviour
{
    [Header("Url Text")]
    [Space]
    public string sendImageToServerURL = "http://your-node-js-server-url:3000/upload";
    public string _imageUrl = "http://your-node-js-server-url:3000/ImageForGame1/";
    private bool _done = false;
    private int _downloadImage = 51;
    [Space]

    [Header("Random Monster Image")]
    [Space]
    [SerializeField] private RawImage _monsterImage;
    [SerializeField] private SpriteRenderer _monsterSprite;
    private float SetWidth = 2;
    private float setHeight = 2;
    [SerializeField] private GameObject _scaleMonsterObject;
    [Space]

    [Header("static")]
    [Space]
    public static List<Texture2D> _myTexture = new List<Texture2D>();
    public static bool RandomList = true;
    [SerializeField] private static int currentIndex = 0;
    [SerializeField] private static int _numberCount = 0;
    
    [Header("List Texture")]
    [Space]
    [SerializeField] private static List<Texture2D> _dataTexture = new List<Texture2D>();
    [SerializeField] private List<Texture2D> _CHECK_DATA_TEXTURE_2D = new List<Texture2D>();
    [SerializeField] private List<Texture2D> _CHECKtexture2D = new List<Texture2D>();

    public void GetData(string json)
    {
        GameData jsonObjects = JsonUtility.FromJson<GameData>(json);
        string _fullUrlPart = $"http://{jsonObjects.IP}:3000/upload";
        sendImageToServerURL = _fullUrlPart;

        _imageUrl = $"http://{jsonObjects.IP}:3000/ImageForGame1/";

        _downloadImage = jsonObjects.DownloadImage;
    }

    private void Awake()
    {
        if(RandomList)
        {
            for (int i = 0; i < _downloadImage; i++)
            {
                StartCoroutine(LoadImage(_imageUrl + i + ".png"));

                if (i == _downloadImage - 1)
                {
                    Debug.Log("Downloads All Image susscful");
                    _done = true;
                }
            }

            RandomList = false;
            StartCoroutine(delay());
        }

        else if (!RandomList)
        {
            setUpTexture();
        }

        _CHECK_DATA_TEXTURE_2D = _dataTexture;
    }

    private IEnumerator delay()
    {
        yield return new WaitUntil(() => _done);
        yield return new WaitForSeconds(1.0F);

        _myTexture = new List<Texture2D>(_dataTexture);

        Shuffle(_myTexture);

        setUpTexture();
    }
    
    private void setUpTexture()
    {
        _CHECKtexture2D = _myTexture;

        _monsterImage.texture = _myTexture[currentIndex];

        _monsterSprite.sprite = Sprite.Create(_myTexture[currentIndex], new Rect(0, 0, _myTexture[currentIndex].width, _myTexture[currentIndex].height), new Vector2(0.5f, 0.5f));

        Vector3 scale = transform.localScale;
        scale.x = SetWidth;
        scale.y = setHeight;
        _scaleMonsterObject.transform.localScale = scale;
    }

    private IEnumerator LoadImage(string Url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(Url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // แสดงรูปภาพใน RawImage
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                _dataTexture.Add(texture);
            }
        }
    }

    #region Random List

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    #endregion

    public void GetNextRandomNumber()
    {
        if(_myTexture.Count == 0)
        {
            LoadNewTextureList();
        }

        if (currentIndex < _myTexture.Count)
        {
            UploadImage(currentIndex);
            _myTexture.RemoveAt(currentIndex);

            if (_myTexture.Count == 0)
            {
                LoadNewTextureList();
            }
            else
            {
                _monsterImage.texture = _myTexture[currentIndex];
                _monsterSprite.sprite = Sprite.Create(_myTexture[currentIndex], new Rect(0, 0, _myTexture[currentIndex].width, _myTexture[currentIndex].height), new Vector2(0.5f, 0.5f));
                Vector3 scale = transform.localScale;
                scale.x = SetWidth;
                scale.y = setHeight;
                _scaleMonsterObject.transform.localScale = scale;
            }
        }
    }

    private void LoadNewTextureList()
    {
        _myTexture = new List<Texture2D>(_dataTexture);
        Shuffle(_myTexture);
        setUpTexture();
    }

    public void UploadImage(int indexNumber)
    {
        // Use Texture2D ARGB32 Befor EncodeToPNG
        Texture2D convertedImage = new Texture2D(_myTexture[indexNumber].width, _myTexture[indexNumber].height, TextureFormat.ARGB32, false);
        convertedImage.SetPixels(_myTexture[indexNumber].GetPixels());
        convertedImage.Apply();
        StartCoroutine(UploadImageToServer(_myTexture[indexNumber]));
    }

    private IEnumerator UploadImageToServer(Texture2D image)
    {
        byte[] imageData = image.EncodeToPNG();
        WWWForm form = new WWWForm();

        form.AddBinaryData("image", imageData, _numberCount + 1 + ".png", "image/png");
        Debug.Log(_numberCount + 1);

        if(_numberCount <= 49)
        {
            _numberCount++;
        }
        else
            _numberCount = 0;

        UnityWebRequest request = UnityWebRequest.Post(sendImageToServerURL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Image uploaded successfully");
        }
        else
        {
            Debug.LogError("Error uploading image: " + request.error);
        }
    }
}
