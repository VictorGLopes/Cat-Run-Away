using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;

   public void UpdateCoins(int coin)
    {
        coinText.text = coin.ToString();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score + "m";
    }
}
