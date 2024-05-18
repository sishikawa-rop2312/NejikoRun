using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleController : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        // ハイスコアを表示
        highScoreText.text = $"High Score : {PlayerPrefs.GetInt("HighScore")}m";
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
}
