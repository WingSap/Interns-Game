using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TextMovement : MonoBehaviour
{
    public PlayableDirector _myTimeLine;

    private void Update()
    {
        _myTimeLine.Play();
    }
}
