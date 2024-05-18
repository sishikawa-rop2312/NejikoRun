using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public TextMeshProUGUI scoreText;
    public LifePanel lifePanel;

    void Update()
    {
        // スコア更新
        int score = CalcScore();
        scoreText.text = "Score : " + score + "m";

        // ライフパネルを更新
        lifePanel.UpdateLife(nejiko.Life());
    }

    int CalcScore()
    {
        // ネジコの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }
}
