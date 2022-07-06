using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text waveText;
    public Text scoreText;

    public Text gameWaveText;
    public Text gameScoreText;

    void Start()
    {
        waveText.text = "Waves: " + Stats.instance.waves;
        scoreText.text = "Score: " + Stats.instance.score;

        gameWaveText.enabled = false;
        gameScoreText.enabled = false;
    }
}
