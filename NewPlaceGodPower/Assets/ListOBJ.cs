using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="TextureData" , menuName = "MyOBJ/TextureData")]
public class ListOBJ : ScriptableObject
{

    public List<Texture2D> _myTexture = new List<Texture2D>();

}
