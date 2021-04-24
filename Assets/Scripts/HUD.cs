using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // HUD Variables
    public Text healthText;
    public Text scoreText;

    public int _score;
    public int Score { 
        get {
            return _score;
        }
        set
        {
            _score = value;
            scoreText.text = $"Kills: {Score}";
        }
    }

    public void AddScore(int amt)
    {
        Score += amt;
    }
}
