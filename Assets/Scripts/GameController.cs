using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

        // ネジコのライフが0になったらゲームオーバー
        if (nejiko.Life() <= 0)
        {
            // これ以降のUpdateは止まる
            enabled = false;

            // ハイスコアを更新
            // PlayerPrefsクラスで簡易的なセーブデータを扱うことができる（float, int, string型しか保存できない）
            if (PlayerPrefs.GetInt("HighScore") < score)
            {
                PlayerPrefs.SetInt("HighScore", score);
            }

            // 2秒後にReturnToTitleを呼び出す
            Invoke("ReturnToTitle", 2.0f);
        }
    }

    int CalcScore()
    {
        // ネジコの走行距離をスコアとする
        return (int)nejiko.transform.position.z;
    }

    void ReturnToTitle()
    {
        // タイトルシーンに切り替え
        SceneManager.LoadScene("Title");
    }
}
