using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextMeshProUGUI;

    [SerializeField] private static int m_MaxScore;

    private void Start()
    {
        m_TextMeshProUGUI.text = "Best Score : " + m_MaxScore.ToString();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(WaveSpawner.score >= m_MaxScore)
        {
            m_MaxScore = WaveSpawner.score;
        }
    }

    private void OnGUI()
    {
        m_TextMeshProUGUI.text = "Best Score : " + m_MaxScore.ToString();
    }
}
