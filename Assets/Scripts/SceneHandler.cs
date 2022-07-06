using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static bool gameOver;

    public GameObject lifeContainer;
    public Text scoreText;
    public GameObject startGameButton;
    public GameObject options;

    int maxLives;
    int lives;
    HorizontalLayoutGroup lifeGroup;
    public GameObject gameOverScreen;
    bool once = true;

    public static SceneHandler instance;

    void Start()
    {
        instance = this;
        gameOver = false;

        maxLives = lifeContainer.transform.childCount;
        lives = maxLives;

        lifeGroup = lifeContainer.GetComponent<HorizontalLayoutGroup>();
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (lives <= 0 && once)
        {
            once = false;
            EndGame();

        }

        // input listener
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            options.SetActive(!options.activeSelf);
        }
    }

    public void DecreaseLives()
    {
        if (lives <= 0)
        {
            return;
        }

        lives -= 1;
        Destroy(lifeGroup.transform.GetChild(lives).gameObject);
    }

    private void EndGame()
    {
        gameOver = true;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        gameOverScreen.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetScore(int value)
    {
        Stats.instance.score += value;
        int score = Stats.instance.score;

        scoreText.text = "Score: " + (score >= 100000 ? score.ToString("e1") : score.ToString());
    }

    public void StartGame()
    {
        Spawner spawner = GetComponent<Spawner>();

        spawner.enabled = true;
        startGameButton.SetActive(false);
    }
}
